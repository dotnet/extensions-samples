// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.Metrics;
using Microsoft.Extensions.Diagnostics.Metrics;

namespace Metrics.Generators;

// The class defines the metrics that will be used by the application.
// We define only metrics definitions here, the actual metrics are created in the generated code.
internal sealed partial class Metric
{
    internal static class MetricNames
    {
        public const string RequestStats = "sample.request_stats";
        public const string TotalRequests = "sample.total_requests";
        public const string FailedRequests = "sample.failed_requests";
    }

    internal static class Tags
    {
        public const string Target = nameof(Target);
        public const string FailureReason = nameof(FailureReason);
        public const string DayOfWeek = nameof(DayOfWeek);
    }

    // This shows how to define a histogram metric with tags based on some RequestInfo.
    // All tags for this metric will be automatically generated from the the properties
    // of the RequestInfo which are annotated with the [TagName] attribute.
    [Histogram(typeof(RequestInfo), Name = MetricNames.RequestStats)]
    public static partial RequestStatsHistogram CreateRequestStatsHistogram(Meter meter);

    // This shows how to define a counter metric with a single tag.
    [Counter(Tags.Target, Name = MetricNames.TotalRequests)]
    public static partial TotalRequestCounter CreateTotalRequestCounter(Meter meter);

    // This shows how to define a counter metric with two tags.
    [Counter(Tags.Target, Tags.FailureReason, Name = MetricNames.FailedRequests)]
    public static partial FailedRequestCounter CreateFailedRequestCounter(Meter meter);
}
