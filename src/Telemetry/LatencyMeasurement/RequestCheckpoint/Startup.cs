// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Telemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Telemetry.Latency;
using RequestCheckpoint.Middleware;

namespace RequestCheckpoint;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Startup class")]
internal class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // HACK!
        var t = typeof(ILatencyContext).Assembly.GetTypes().First(t => t.Name == "NullLatencyContext");
        services.TryAddSingleton(typeof(ILatencyContext), t);

        _ = services
            .AddRequestCheckpoint()
            .AddNullLatencyContext()
            .AddSingleton<FirstMiddleware>()
            .AddSingleton<SecondMiddleware>()
            .AddSingleton<ThirdMiddleware>()
            .AddControllersWithViews()
            .Services
            .AddRouting();
    }

    public void Configure(IApplicationBuilder app)
    {
        _ = app
            .UseMiddleware<FirstMiddleware>()
            .UseRequestCheckpoint()
            .UseMiddleware<SecondMiddleware>()
            .UseRouting()
            .UseMiddleware<ThirdMiddleware>()
            .UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
    }
}
