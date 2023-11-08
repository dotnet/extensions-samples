// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.Diagnostics.Metrics;
using static Metrics.Generators.Metric;

namespace Metrics.Generators;

internal struct RequestInfo
{
    [TagName(Dimensions.Target)]
    public RequestTarget Target { get; set; }

    [TagName(Dimensions.DayOfWeek)]
    public DayOfWeek DayOfWeek { get; set; }
}
