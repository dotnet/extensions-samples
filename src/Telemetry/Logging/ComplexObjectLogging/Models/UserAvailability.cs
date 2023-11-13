// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Shared.Compliance;

namespace ComplexObjectLogging.Models;

internal sealed class UserAvailability(string username, string status)
{
    // We annotate sensitive properties with corresponding attributes
    // so that they will be redacted when logging.
    [PrivateData]
    public string Username { get; } = username;

    public string Status { get; } = status;
}
