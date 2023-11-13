// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Diagnostics.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.Enrichment;

namespace HttpLogging.Enrichment;

#pragma warning disable EXTEXP0013 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
// HTTP log enrichers are used to add additional information to the HTTP logs.
internal sealed class HttpLogEnricher : IHttpLogEnricher
#pragma warning restore EXTEXP0013 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
{
    public void Enrich(IEnrichmentTagCollector collector, HttpContext httpContext)
    {
        // Add additional information to the HTTP logs so that they will have more value.
        collector.Add("Response.Headers.Count", httpContext.Response.Headers.Count);
    }
}
