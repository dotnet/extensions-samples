// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.Latency;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace LatencyContext;

internal sealed class ConsoleLatencyDataExporter : ILatencyDataExporter
{
    private readonly ILogger _logger;
    private readonly ObjectPool<StringBuilder> _builderPool = new DefaultObjectPool<StringBuilder>(
        new StringBuilderPooledObjectPolicy
        {
            InitialCapacity = 128,
            MaximumRetainedCapacity = 1024
        }, 1024);

    private readonly LogLevel _level;

    public ConsoleLatencyDataExporter(IOptions<ConsoleLatencyDataExporterOptions> options, ILogger<ConsoleLatencyDataExporter> logger)
    {
        _logger = logger;
        _level = options.Value.LogLevel;
    }

    /// <summary>
    /// Function called to export latency data.
    /// </summary>
    /// <param name="latencyData">Latency data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/>that represents the export operation.</returns>
    public Task ExportAsync(LatencyData latencyData, CancellationToken cancellationToken)
    {
        if (_logger.IsEnabled(_level))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var stringBuilder = _builderPool.Get();
            try
            {
                FormatLog(stringBuilder, latencyData);
                Log.Latency(_logger, _level, stringBuilder);
            }
            finally
            {
                _builderPool.Return(stringBuilder);
            }
        }

        return Task.CompletedTask;
    }

    private static void FormatLog(StringBuilder sb, LatencyData latencyData)
    {
        // Append tags
        AppendSpan(sb, latencyData.Tags, a => a.Name);
        _ = sb.Append(',');
        AppendSpan(sb, latencyData.Tags, a => a.Value);
        _ = sb.Append(',');

        // Append checkpoints
        AppendSpan(sb, latencyData.Checkpoints, a => a.Name);
        _ = sb.Append(',');
        AppendSpan(sb, latencyData.Checkpoints, a => (long)a.Elapsed);
        _ = sb.Append(',');

        // Append measures
        AppendSpan(sb, latencyData.Measures, a => a.Name);
        _ = sb.Append(',');
        AppendSpan(sb, latencyData.Measures, a => a.Value);
        _ = sb.Append(',');

        // Append duration
        _ = sb.Append((long)latencyData.DurationTimestamp);
    }

    private static void AppendSpan<TX, TY>(StringBuilder sb, ReadOnlySpan<TX> span, Func<TX, TY> select)
    {
        for (var i = 0; i < span.Length; i++)
        {
            _ = sb.Append(select(span[i]));
            _ = sb.Append('/');
        }
    }
}
