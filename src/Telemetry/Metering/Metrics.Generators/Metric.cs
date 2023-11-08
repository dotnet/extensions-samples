// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.Metrics;
using Microsoft.Extensions.Diagnostics.Metrics;

namespace Metrics.Generators;

internal sealed partial class Metric
{
    internal static class MetricNames
    {
        public const string RequestStats = "sample.request_stats";
        public const string TotalRequests = "sample.total_requests";
        public const string FailedRequests = "sample.failed_requests";
    }

    internal static class Dimensions
    {
        public const string Target = nameof(Target);
        public const string FailureReason = nameof(FailureReason);
        public const string DayOfWeek = nameof(DayOfWeek);
    }

    [Histogram(typeof(RequestInfo), Name = MetricNames.RequestStats)]
    public static partial RequestStatsHistogram CreateRequestStatsHistogram(Meter meter);

    [Counter(Dimensions.Target, Name = MetricNames.TotalRequests)]
    public static partial TotalRequestCounter CreateTotalRequestCounter(Meter meter);

    [Counter(Dimensions.Target, Dimensions.FailureReason, Name = MetricNames.FailedRequests)]
    public static partial FailedRequestCounter CreateFailedRequestCounter(Meter meter);
}
