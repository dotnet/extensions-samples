// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Metrics.Generators;

internal static class Startup
{
    public static async Task Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddHostedService<TelemetryEmitterBackgroundService>())
            .Build();

        await host.RunAsync().ConfigureAwait(false);
    }
}
