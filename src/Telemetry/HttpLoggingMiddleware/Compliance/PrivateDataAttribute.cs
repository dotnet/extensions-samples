// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.Compliance.Classification;

namespace HttpLogging.Compliance;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
internal sealed class PrivateDataAttribute : DataClassificationAttribute
{
    public PrivateDataAttribute()
        : base(DataTaxonomy.PrivateData)
    {
    }
}
