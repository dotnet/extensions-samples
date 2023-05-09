// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace LatencyContext;

/// <summary>
/// Options for <see cref="ConsoleLatencyDataExporter"/>.
/// </summary>
internal sealed class ConsoleLatencyDataExporterOptions
{
    /// <summary>
    /// Gets or sets level for logging latency data.
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}
