// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.Metrics;
using Microsoft.Extensions.Diagnostics.Metrics;

namespace Metrics.Generators;

// The class defines the metrics that will be used by the application.
// We define only metrics definitions here, the actual metrics are created in the generated code.
// XML docs below are needed to generate the metrics report.
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
        /// <summary>
        /// Target service being requested.
        /// </summary>
        public const string Target = nameof(Target);

        /// <summary>
        /// The reason of the failure.
        /// </summary>
        public const string FailureReason = nameof(FailureReason);

        /// <summary>
        /// Day of the week.
        /// </summary>
        public const string DayOfWeek = nameof(DayOfWeek);
    }

    // This shows how to define a counter metric with a single tag.
    /// <summary>
    /// Counter for the total number of requests.
    /// </summary>
    [Counter(Tags.Target, Name = MetricNames.TotalRequests)]
    public static partial TotalRequestCounter CreateTotalRequestCounter(Meter meter);

    // This shows how to define a counter metric with two tags.
    /// <summary>
    /// Counter for the total number of failed requests.
    /// </summary>
    [Counter(Tags.Target, Tags.FailureReason, Name = MetricNames.FailedRequests)]
    public static partial FailedRequestCounter CreateFailedRequestCounter(Meter meter);

    // This shows how to define a histogram metric with tags based on the RequestInfo type.
    // All tags for this metric will be automatically generated from the the properties
    // of the RequestInfo type which are annotated with the [TagName] attribute.
    /// <summary>
    /// Histogram for request statistics.
    /// </summary>
    [Histogram(typeof(RequestInfo), Name = MetricNames.RequestStats)]
    public static partial RequestStatsHistogram CreateRequestStatsHistogram(Meter meter);
}
