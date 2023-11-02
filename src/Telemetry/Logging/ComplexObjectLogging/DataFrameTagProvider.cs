// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using ComplexObjectLogging.Models;
using Microsoft.Extensions.Logging;

namespace ComplexObjectLogging;

internal static class DataFrameTagProvider
{
    // A signature of a method that will be used to provide tags for a given data frame should match to this one.
    public static void Provide(ITagCollector collector, DataFrame dataFrame)
    {
        // You can provide any custom name for a tag if you want to.
        collector.Add("DataFrameType", dataFrame.Type.ToString(CultureInfo.InvariantCulture));
        collector.Add(nameof(dataFrame.PayloadLength), dataFrame.PayloadLength.ToString(CultureInfo.InvariantCulture));

        // You can also use any logic to determine whether a tag should be added or not:
        if (dataFrame.StreamId != 0)
        {
            collector.Add(nameof(dataFrame.StreamId), dataFrame.StreamId.ToString(CultureInfo.InvariantCulture));
        }
    }
}
