// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Compliance.Testing;
using Microsoft.Extensions.Logging;

namespace ComplianceTesting
{
    internal static partial class Log
    {
        [LoggerMessage(LogLevel.Information, "User with {Username} got created.")]
        public static partial void UserCreated(this ILogger logger, [PrivateData] string username);
    }
}
