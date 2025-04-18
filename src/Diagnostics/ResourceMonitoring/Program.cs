// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;

namespace ResourceMonitoring;

internal static class Program
{
    public static async Task Main()
    {
        await Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                _ = services.AddOpenTelemetry()
                    .WithMetrics(meterProviderBuilder => meterProviderBuilder
                    .AddMeter("Microsoft.Extensions.Diagnostics.ResourceMonitoring")
                    .AddConsoleExporter());

                _ = services.AddResourceMonitoring();
            })
            .Build()
            .RunAsync()
            .ConfigureAwait(false);
    }
}
