// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Diagnostics.Metrics;
using static Metrics.Generators.Metric;

namespace Metrics.Generators;

// The structure that contains the metering information about the request.
// XML docs for type's members are needed to generate the metrics report.
internal struct RequestInfo
{
    // This annotated property will be used as a tag for the RequestStats histogram.
    /// <summary>
    /// Target service being requested.
    /// </summary>
    [TagName(Tags.Target)]
    public RequestTarget Target { get; set; }

    // This annotated property will be used as a tag for the RequestStats histogram.
    // You can omit the [TagName] attribute if the tag name is the same as the property name.
    /// <summary>
    /// Day of the week.
    /// </summary>
    public string DayOfWeek { get; set; }
}
