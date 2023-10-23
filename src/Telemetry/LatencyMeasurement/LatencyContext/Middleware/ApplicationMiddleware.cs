// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Latency;

namespace LatencyContext
{
    internal sealed class ApplicationMiddleware : IMiddleware
    {
        private readonly TimeSpan _secondsDelay = TimeSpan.FromSeconds(1);
        private readonly int _cpuTime = 5;
        private readonly TagToken _region;
        private readonly TagToken _sku;
        private readonly TagToken _api;
        private readonly CheckpointToken _start;
        private readonly CheckpointToken _end;
        private readonly MeasureToken _cpu;

        public ApplicationMiddleware(ILatencyContextTokenIssuer latencyContextTokenIssuer)
        {
            _region = latencyContextTokenIssuer.GetTagToken(ApplicationTagConstants.Region);
            _sku = latencyContextTokenIssuer.GetTagToken(ApplicationTagConstants.HardwareSKU);
            _api = latencyContextTokenIssuer.GetTagToken(ApplicationTagConstants.API);

            _start = latencyContextTokenIssuer.GetCheckpointToken(ApplicationCheckpointConstants.AppMiddlewareStart);
            _end = latencyContextTokenIssuer.GetCheckpointToken(ApplicationCheckpointConstants.AppMiddlewareEnd);

            _cpu = latencyContextTokenIssuer.GetMeasureToken(ApplicationMeasureConstants.CpuTime);
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var latencyContext = context.RequestServices.GetRequiredService<ILatencyContext>();

            // Checkpoint
            latencyContext.AddCheckpoint(_start);

            // Tags
            latencyContext.SetTag(_region, "NAM");
            latencyContext.SetTag(_sku, "Gen9");
            latencyContext.SetTag(_api, "FetchData");

            // Logic
            await next(context).ConfigureAwait(false);
            await Task.Delay(_secondsDelay, context.RequestAborted).ConfigureAwait(false);

            // Measure
            latencyContext.RecordMeasure(_cpu, _cpuTime);

            // Checkpoint
            latencyContext.AddCheckpoint(_end);
        }
    }
}
