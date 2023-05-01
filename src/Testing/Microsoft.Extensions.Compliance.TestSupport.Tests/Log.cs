// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Compliance.Classification.Simple;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Telemetry.Logging;

namespace FakeRedaction
{
    internal static partial class Log
    {
        [LogMethod(0, LogLevel.Information, "User with {username} got created.")]
        public static partial void UserCreated(this ILogger logger, IRedactorProvider redactor, [PrivateData] string username);
    }
}
