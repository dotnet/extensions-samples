// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace ComplexObjectLogging;

internal static partial class Log
{
    // We only need to define logging method's signature and the source code generator will do the rest.
    // Notice that we annotated the "attributes" parameter with [LogProperties] attribute to log its properties.
    [LoggerMessage(Level = LogLevel.Information, Message = "Starting up the application")]
    public static partial void StartingUp(ILogger logger, [LogProperties] StartupAttributes attributes);
}
