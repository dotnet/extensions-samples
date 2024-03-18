// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace ComplexObjectLogging.Models;

internal sealed class StartupAttributes(string machineName, string applicationName)
{
    public string MachineName { get; } = machineName;

    public string ApplicationName { get; } = applicationName;
}
