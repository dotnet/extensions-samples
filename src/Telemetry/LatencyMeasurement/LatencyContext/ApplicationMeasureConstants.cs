// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace LatencyContext
{
    internal static class ApplicationMeasureConstants
    {
        public const string DBTime = "dbt";
        public const string ItemsCacheCount = "icc";
        public const string TotalItemCount = "tic";
        public const string CpuTime = "cput";

        internal static readonly string[] Measures = new[]
        {
            DBTime,
            ItemsCacheCount,
            TotalItemCount,
            CpuTime
        };
    }
}
