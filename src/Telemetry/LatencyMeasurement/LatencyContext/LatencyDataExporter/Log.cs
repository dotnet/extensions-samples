// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Microsoft.Extensions.Logging;

namespace LatencyContext;

internal static partial class Log
{
    [LoggerMessage(0, "{message}")]
    public static partial void Latency(ILogger logger, LogLevel level, StringBuilder message);
}
