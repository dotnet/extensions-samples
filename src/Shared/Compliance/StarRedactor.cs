// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.Compliance.Redaction;

namespace Shared.Compliance;

// The implementation below is intended for demonstration purposes only and should NOT be used in production environments
internal sealed class StarRedactor : Redactor
{
    private const char Replacement = '*';

    public override int GetRedactedLength(ReadOnlySpan<char> source)
        => source.Length;

    public override int Redact(ReadOnlySpan<char> source, Span<char> destination)
    {
        var len = source.Length;
        var redacted = new string(Replacement, len);
        redacted.AsSpan().CopyTo(destination);
        return len;
    }
}
