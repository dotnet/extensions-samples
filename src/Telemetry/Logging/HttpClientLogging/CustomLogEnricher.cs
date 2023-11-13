// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net.Http;
using Microsoft.Extensions.Diagnostics.Enrichment;
using Microsoft.Extensions.Http.Logging;

// A custom log enricher that adds tags to HttpClient logs.
// An enricher should implement IHttpClientLogEnricher.
internal sealed class CustomLogEnricher : IHttpClientLogEnricher
{
    public void Enrich(IEnrichmentTagCollector collector, HttpRequestMessage request, HttpResponseMessage? response, Exception? exception)
    {
        // We can add a custom tag to the log:
        collector.Add("Custom-Tag", "Custom Value");

        // Or we can add a tag based on the request data:
        collector.Add("network.protocol.version", request.Version);
    }
}
