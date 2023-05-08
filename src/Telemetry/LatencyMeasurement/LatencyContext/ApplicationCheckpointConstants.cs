// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace LatencyContext;

internal static class ApplicationCheckpointConstants
{
    public const string AppMiddlewareStart = "ams";
    public const string AppMiddlewareEnd = "ame";
    public const string DataMiddleareStart = "dms";
    public const string DataMiddlewareEnd = "dme";

    internal static readonly string[] Checkpoints = new[]
    {
        AppMiddlewareStart,
        AppMiddlewareEnd,
        DataMiddleareStart,
        DataMiddlewareEnd
    };
}
