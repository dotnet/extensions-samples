// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Compliance.Classification;

namespace HttpClientLogging;

internal static class DataTaxonomy
{
    public static DataClassification PrivateData => new("ComplianceTaxonomy", "Private");

    public static DataClassification PublicData => new("ComplianceTaxonomy", "Public");
}
