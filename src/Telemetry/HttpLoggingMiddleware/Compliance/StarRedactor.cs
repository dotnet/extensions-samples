// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.Compliance.Redaction;

namespace HttpLogging.Compliance;

internal sealed class StarRedactor : Redactor
{
    private const char Replacement = '*';

    public override int GetRedactedLength(ReadOnlySpan<char> source)
    {
        return source.Length;
    }

    public override int Redact(ReadOnlySpan<char> source, Span<char> destination)
    {
        var redacted = new string(Replacement, source.Length);
        redacted.AsSpan().CopyTo(destination);
        return source.Length;
    }
}
