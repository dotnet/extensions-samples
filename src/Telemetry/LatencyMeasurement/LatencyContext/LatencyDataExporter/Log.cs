// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Telemetry.Logging;

namespace LatencyContext;

internal static partial class Log
{
    [LogMethod(0, "{message}")]
    public static partial void Latency(ILogger logger, LogLevel level, StringBuilder message);
}
