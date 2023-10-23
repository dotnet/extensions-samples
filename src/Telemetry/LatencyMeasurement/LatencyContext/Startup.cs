// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LatencyContext
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Startup class")]
    internal sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services
                .AddRouting()
                .AddLatencyContext() // adds an implementation of ILatencyContext and ILatencyContextControl
                .RegisterCheckpointNames(ApplicationCheckpointConstants.Checkpoints)
                .RegisterMeasureNames(ApplicationMeasureConstants.Measures)
                .RegisterTagNames(ApplicationTagConstants.Tags)

                // Optional services that work with latency context
                .AddRequestLatencyTelemetry(o => o.LatencyDataExportTimeout = System.TimeSpan.FromSeconds(500.0)) // manages the ILatencyContext via ILatencyContextControl

                .AddConsoleLatencyDataExporter() // exports ILatencyData using console
                .AddLogging(x => x.AddConsole()) // used by ConsoleLatencyDataExporter

                // Application logic
                .AddSingleton<ApplicationMiddleware>();
        }

        public void Configure(IApplicationBuilder app)
        {
            _ = app.UseRouting()
               .UseRequestLatencyTelemetry()
               .UseMiddleware<ApplicationMiddleware>()
               .UseMiddleware<DataMiddleware>()
               .UseEndpoints(endpoints =>
                {
                    _ = endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync(@$"
                        Welcome!

                        This is a sample application that measures latency using LatencyContext.
                        It also exports the latency data to console (dev-time) through PerfPanelLatencyDataExporter.

                        It consists of two middlewares that add tags, checkpoints and measures.").ConfigureAwait(false);
                    });
                });
        }
    }
}
