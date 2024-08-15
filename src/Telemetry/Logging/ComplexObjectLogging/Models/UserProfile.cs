// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;
using System;
using Shared.Compliance;

namespace ComplexObjectLogging.Models;

public record User
{
    public User(string Id, string Name, string Email, InnerUserData innerData)
    {
        this.Id = Id;
        this.Name = Name;
        this.Email = Email;
        this.InnerData = innerData;
    }

    public string Id { get; }

    [PrivateData]
    public string Name { get; }

    [PrivateData]
    public string Email { get; }

    public InnerUserData InnerData { get; }
}

public record InnerUserData
{
    [PrivateData]
    public string RedactedData { get; } = Guid.NewGuid().ToString();

    public string PublicData { get; } = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
}
