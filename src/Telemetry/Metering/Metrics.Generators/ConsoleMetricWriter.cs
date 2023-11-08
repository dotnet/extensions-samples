// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace Metrics.Generators;

internal sealed class ConsoleMetricWriter : IDisposable
{
    public MeterListener _meterListener;

    public ConsoleMetricWriter()
    {
        _meterListener = new();
        _meterListener.InstrumentPublished = (instrument, listener) =>
        {
            listener.SetMeasurementEventCallback<int>(PrintMeasurement);
            listener.SetMeasurementEventCallback<long>(PrintMeasurement);
            listener.EnableMeasurementEvents(instrument);
        };
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
