// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace ComplexObjectLogging.Models;

internal readonly record struct DataFrame(
    int PayloadLength,
    byte Type,
    int StreamId);
