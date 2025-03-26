// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace LogSampling;

internal static partial class Log
{
    [LoggerMessage(Level = LogLevel.Error, Message = "ERROR log message in my application.")]
    public static partial void ErrorMessage(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "INFORMATION log message in my application.")]
    public static partial void InformationMessage(ILogger logger);

}
