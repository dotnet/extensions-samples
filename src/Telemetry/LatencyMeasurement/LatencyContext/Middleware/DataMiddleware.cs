// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.Latency;

namespace LatencyContext;

internal sealed class DataMiddleware
{
    private readonly TimeSpan _secondsDelay = TimeSpan.FromSeconds(1);
    private readonly RequestDelegate _next;
    private readonly TagToken _userType;
    private readonly CheckpointToken _start;
    private readonly CheckpointToken _end;
    private readonly MeasureToken _dbTime;
    private readonly MeasureToken _itemCount;
    private readonly MeasureToken _itemCacheCount;

    public DataMiddleware(RequestDelegate next, ILatencyContextTokenIssuer latencyContextTokenIssuer)
    {
        _next = next;

        _userType = latencyContextTokenIssuer.GetTagToken(ApplicationTagConstants.UserType);

        _start = latencyContextTokenIssuer.GetCheckpointToken(ApplicationCheckpointConstants.DataMiddleareStart);
        _end = latencyContextTokenIssuer.GetCheckpointToken(ApplicationCheckpointConstants.DataMiddlewareEnd);

        _dbTime = latencyContextTokenIssuer.GetMeasureToken(ApplicationMeasureConstants.DBTime);
        _itemCount = latencyContextTokenIssuer.GetMeasureToken(ApplicationMeasureConstants.TotalItemCount);
        _itemCacheCount = latencyContextTokenIssuer.GetMeasureToken(ApplicationMeasureConstants.ItemsCacheCount);
    }

    public async Task InvokeAsync(HttpContext context, ILatencyContext latencyContext)
    {
        // Checkpoint
        latencyContext.AddCheckpoint(_start);

        // Tag
        latencyContext.SetTag(_userType, "Enterprise");

        // Logic
        await _next(context).ConfigureAwait(false);
        await Task.Delay(_secondsDelay, context.RequestAborted).ConfigureAwait(false);

        // Measure
        // Get 1 item from DB
        for (int i = 0; i < 10; i++)
        {
            latencyContext.AddMeasure(_dbTime, 10);
            latencyContext.AddMeasure(_itemCount, 1);
        }

        // Get 5 items from cache
        latencyContext.AddMeasure(_itemCount, 5);
        latencyContext.RecordMeasure(_itemCacheCount, 5);

        // Checkpoint
        latencyContext.AddCheckpoint(_end);
    }
}
