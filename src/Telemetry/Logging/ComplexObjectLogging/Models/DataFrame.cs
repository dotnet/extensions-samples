// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace ComplexObjectLogging.Models;

internal readonly struct DataFrame(int payloadLength, byte type, int streamId)
{
    public int PayloadLength { get; } = payloadLength;

    public byte Type { get; } = type;

    public int StreamId { get; } = streamId;
}
