// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace Metrics.Generators;

// The utility class that prints to the console metrics emitted by the application.
internal sealed class ConsoleMetricWriter : IDisposable
{
    private readonly MeterListener _meterListener;

    public ConsoleMetricWriter()
    {
        _meterListener = new()
        {
            InstrumentPublished = (instrument, listener) =>
            {
                // You can remove this check if you want to see all metrics (e.g. HttpClient metrics).
                if (instrument.Meter.Name is not nameof(TelemetryEmitterBackgroundService))
                {
                    return;
                }

                listener.EnableMeasurementEvents(instrument);
            }
        };

        _meterListener.SetMeasurementEventCallback<long>(PrintMeasurement);

        _meterListener.Start();
    }

    private static void PrintMeasurement<T>(Instrument instrument, T measurement, ReadOnlySpan<KeyValuePair<string, object?>> tags, object? state)
    {
        var stringBuilder = new StringBuilder()
            .Append("Metric [")
            .Append(instrument.Name)
            .Append("] value: ")
            .Append(measurement);

        foreach (var tag in tags)
        {
            stringBuilder
                .Append(' ')
                .Append(tag.Key)
                .Append('=')
                .Append(tag.Value);
        }

        Console.WriteLine(stringBuilder);
    }

    public void Dispose()
    {
        _meterListener.Dispose();
    }
}
