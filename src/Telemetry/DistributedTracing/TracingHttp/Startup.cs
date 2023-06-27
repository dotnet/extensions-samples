// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Telemetry;
using Microsoft.Extensions.AmbientMetadata;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Compliance.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Telemetry.Tracing;
using Microsoft.Extensions.Telemetry.Enrichment;
using OpenTelemetry.Trace;

namespace TracingHttp;

internal sealed class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Configure redaction. XXHashRedactor is used as an example
        _ = services.AddRedaction(redaction => redaction.SetXXHash3Redactor(_ => { }, SimpleClassifications.PrivateData));

        _ = services.AddRouting();

        _ = services.AddSingleton<ITracingService, TracingService>();

        _ = services.AddControllersWithViews();

        _ = services.AddHttpClient();

        // Adding service metadata information using appsettings.json configuration file:
        _ = services.AddApplicationMetadata(Configuration.GetSection("ServiceMetadata"));

        _ = services.AddOpenTelemetry()
            .WithTracing(builder => builder
            .SetSampler(new AlwaysOnSampler())
            .AddSource(nameof(TracingService))

            // Adding service enrichment to traces
            // ApplicationName and EnvironmentName are enabled by default,
            // so enabling BuildVersion and DeploymentRing to ensure traces are enriched with them as well.
            .AddServiceTraceEnricher(options =>
            {
                options.BuildVersion = true;
                options.DeploymentRing = true;
            })

            .AddHttpTracing(options =>
            {
                options.RouteParameterDataClasses.Add("chatId", SimpleClassifications.PrivateData);
                options.ExcludePathStartsWith.Add("home");
            })
            .AddHttpClientTracing(options =>
            {
                options.RouteParameterDataClasses.Add("chatId", SimpleClassifications.PrivateData);
            })
            .AddHttpTraceEnricher<DayOfTheWeekHttpEnricher>()
            .AddHttpClientTraceEnricher<DayOfTheWeekHttpClientEnricher>()
            .AddConsoleExporter()); // Console exporter is for demo purposes.
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Startup class")]
    public void Configure(IApplicationBuilder app)
        => app.UseRouting()
              .UseEndpoints(endpoints => _ = endpoints.MapDefaultControllerRoute());
}
