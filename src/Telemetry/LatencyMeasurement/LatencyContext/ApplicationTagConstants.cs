// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace LatencyContext
{
    internal static class ApplicationTagConstants
    {
        public const string API = "api";
        public const string HardwareSKU = "sku";
        public const string UserType = "ut";
        public const string Region = "reg";

        internal static readonly string[] Tags = new[]
        {
            API,
            HardwareSKU,
            UserType,
            Region
        };
    }
}
