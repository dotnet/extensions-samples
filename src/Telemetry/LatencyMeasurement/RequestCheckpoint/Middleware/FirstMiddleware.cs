// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RequestCheckpoint.Middleware;

internal class FirstMiddleware : IMiddleware
{
    private readonly TimeSpan _secondsDelay = TimeSpan.FromSeconds(1);
    private readonly ILogger<FirstMiddleware> _logger;

    public FirstMiddleware(ILogger<FirstMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("{Middleware} is here and let's wait just {NumSeconds} seconds", nameof(FirstMiddleware), _secondsDelay.TotalSeconds);

        await Task.Delay(_secondsDelay, context.RequestAborted).ConfigureAwait(false);

        await next(context).ConfigureAwait(false);
    }
}
