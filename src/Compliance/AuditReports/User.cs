// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Shared.Compliance;

namespace AuditReports;

internal sealed class User(string username, string status)
{
    // We annotate sensitive properties with corresponding attributes
    // so that they will be emitted in the compliance report.
    [PrivateData]
    public string Username { get; } = username;

    public string Status { get; } = status;
}
