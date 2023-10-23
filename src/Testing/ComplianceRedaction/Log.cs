// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Compliance.Testing;
using Microsoft.Extensions.Logging;

namespace FakeRedaction
{
    internal static partial class Log
    {
        [LoggerMessage(0, LogLevel.Information, "User with {username} got created.")]
        public static partial void UserCreated(this ILogger logger, IRedactorProvider redactor, [PrivateData] string username);
    }
}
