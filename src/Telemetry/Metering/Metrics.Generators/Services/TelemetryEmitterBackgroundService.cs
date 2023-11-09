// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Metrics.Generators;

internal sealed class TelemetryEmitterBackgroundService : IHostedService
{
    private readonly Meter _meter;
    private readonly RequestStatsHistogram _requestsStatsHistogram;
    private readonly TotalRequestCounter _totalRequestCounter;
    private readonly FailedRequestCounter _failedRequestCounter;

    public TelemetryEmitterBackgroundService()
    {
        _meter = new Meter(nameof(TelemetryEmitterBackgroundService));

        // Create metering instruments using the auto-generated code:
        _requestsStatsHistogram = Metric.CreateRequestStatsHistogram(_meter);
        _totalRequestCounter = Metric.CreateTotalRequestCounter(_meter);
        _failedRequestCounter = Metric.CreateFailedRequestCounter(_meter);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            using var consoleMetricWriter = new ConsoleMetricWriter();

            var targets = new List<(RequestTarget, string)>
            {
                new (RequestTarget.MicrosoftDotCom, "https://microsoft.com"),
                new (RequestTarget.GitHubDotCom, "https://github.com"),
                new (RequestTarget.LinkedInDotCom, "https://linkedin.com"),
                new (RequestTarget.Invalid, "invalid_url"),
            };

            using var httpClient = new HttpClient();
            var random = new Random((int)DateTimeOffset.UtcNow.Ticks);

            while (!cancellationToken.IsCancellationRequested)
            {
                var index = random.Next(targets.Count);
                var target = targets[index].Item1;
                var targetUrl = targets[index].Item2;

                try
                {
                    Console.WriteLine($"{Environment.NewLine}Sending request to ${targetUrl}...");

                    // Record the 'sample.total_requests' counter metric.
                    _totalRequestCounter.Add(1, target);

                    var response = await httpClient.GetAsync(targetUrl, cancellationToken).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                        // Record the 'sample.request_stats' histogram metric.
                        _requestsStatsHistogram.Record(
                            content.Length,
                            new RequestInfo
                            {
                                Target = target,
                                DayOfWeek = DateTimeOffset.UtcNow.DayOfWeek
                            });
                    }
                    else
                    {
                        // Record the 'sample.failed_requests' counter metric.
                        _failedRequestCounter.Add(1, target, response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    // Record the 'sample.failed_requests' counter metric.
                    _failedRequestCounter.Add(1, target, ex.GetType().Name);
                }

                await Task.Delay(5000, cancellationToken).ConfigureAwait(false);
            }
        }, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _meter.Dispose();

        return Task.CompletedTask;
    }
}
