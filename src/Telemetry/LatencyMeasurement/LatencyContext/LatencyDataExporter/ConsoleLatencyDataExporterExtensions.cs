// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.Latency;

namespace LatencyContext;

internal static class ConsoleLatencyDataExporterExtensions
{
    /// <summary>
    /// Add latency data exporter.
    /// </summary>
    /// <param name="services">Dependency injection container.</param>
    /// <returns>Provided service collection with <see cref="ConsoleLatencyDataExporter"/> added.</returns>
    public static IServiceCollection AddConsoleLatencyDataExporter(this IServiceCollection services)
    {
        _ = services.AddOptions<ConsoleLatencyDataExporterOptions>();
        services.TryAddSingleton<ILatencyDataExporter, ConsoleLatencyDataExporter>();

        return services;
    }
}
