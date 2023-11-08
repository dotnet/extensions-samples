// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.Metrics;

namespace Metrics.Generators;

internal sealed class TelemetryEmitter : IDisposable
{
    public Meter _meter;
    public RequestStatsHistogram _requestsStatsHistogram;
    public TotalRequestCounter _totalRequestCounter;
    public FailedRequestCounter _failedRequestCounter;

    public TelemetryEmitter()
    {
        _meter = new Meter(nameof(TelemetryEmitter));

        _requestsStatsHistogram = Metric.CreateRequestStatsHistogram(_meter);
        _totalRequestCounter = Metric.CreateTotalRequestCounter(_meter);
        _failedRequestCounter = Metric.CreateFailedRequestCounter(_meter);
    }

    public void RecordRequest(RequestTarget target)
    {
        _totalRequestCounter.Add(1, target);
    }

    public void RecordFailedRequest(RequestTarget target, string reason)
    {
        _failedRequestCounter.Add(1, target, reason);
    }

    public void RecordRequestStats(int responseSize, RequestInfo request)
    {
        _requestsStatsHistogram.Record(responseSize, request);
    }

    public void Dispose()
    {
        _meter.Dispose();
    }
}
