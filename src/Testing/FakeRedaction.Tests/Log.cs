// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.R9.Extensions.Data.Classification;
using Microsoft.R9.Extensions.Logging;
using Microsoft.R9.Extensions.Redaction;

namespace FakeRedaction
{
    internal static partial class Log
    {
        [LogMethod(0, LogLevel.Information, "User with {username} got created.")]
        public static partial void UserCreated(this ILogger logger, IRedactorProvider redactor, EUPI<string> username);
    }
}