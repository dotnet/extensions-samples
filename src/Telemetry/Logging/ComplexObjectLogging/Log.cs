// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using ComplexObjectLogging.Models;
using Microsoft.Extensions.Logging;
using Shared.Compliance;

namespace ComplexObjectLogging;

internal static partial class Log
{
    // We only need to define a logging method's signature and the source generator will do the rest.
    // Notice that we annotated the "attributes" parameter with [LogProperties] attribute to log its properties.
    [LoggerMessage(Level = LogLevel.Information, Message = "Starting up the application")]
    public static partial void StartingUp(ILogger logger, [LogProperties] StartupAttributes attributes);

    // This one shows the usage of a custom tag provider.
    // Please inspect DataFrameTagProvider type to see how it is implemented and what gets logged.
    // We optionally enable OmitReferenceName to remove the parameter name from the log record (e.g. "StreamId" instead of "dataFrame.StreamId").
    [LoggerMessage(Level = LogLevel.Information, Message = "Data frame was sent")]
    public static partial void DataFrameSent(
        ILogger logger,
        [TagProvider(typeof(DataFrameTagProvider), nameof(DataFrameTagProvider.Provide), OmitReferenceName = true)] DataFrame dataFrame);

    // This overload shows how to log an enumerable:
    // Please note that "ILogger" parameter is nullable, the generated implementation won't throw if it is null.
    [LoggerMessage(Level = LogLevel.Debug, Message = "Data frame was sent to {Destinations}")]
    public static partial void DataFrameSent(ILogger? logger, IEnumerable<string> destinations);

    // This method shows how to log sensitive data in a compliant manner by using redaction feature.
    // Please inspect UserAvailability type to see how it is implemented and what gets logged.
    [LoggerMessage(Level = LogLevel.Warning, Message = "User {Name} has now different status")]
    public static partial void UserIdAvailabilityChanged(
        ILogger logger,
        [PrivateData] string name,
        [LogProperties] UserAvailability availability);
}
