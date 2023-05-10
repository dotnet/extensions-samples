// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Telemetry;

namespace TracingHttp;

internal sealed class DayOfTheWeekHttpEnricher : IHttpTraceEnricher
{
    public void Enrich(Activity activity, HttpRequest? request)
    {
        if (request != null)
        {
            _ = activity.SetTag("HttpEnricher_DayOfWeek", TimeProvider.System.GetUtcNow().DayOfWeek);
        }
    }
}
