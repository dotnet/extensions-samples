// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Metrics.Generators;

internal sealed class TelemetryEmitterBackgroundService : BackgroundService
{
    private readonly Meter _meter;
    private readonly RequestStatsHistogram _requestsStatsHistogram;
    private readonly TotalRequestCounter _totalRequestCounter;
    private readonly FailedRequestCounter _failedRequestCounter;
    private readonly PeriodicTimer _timer;

    public TelemetryEmitterBackgroundService()
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        _meter = new Meter(nameof(TelemetryEmitterBackgroundService));

        // Create metering instruments using the auto-generated code:
        _requestsStatsHistogram = Metric.CreateRequestStatsHistogram(_meter);
        _totalRequestCounter = Metric.CreateTotalRequestCounter(_meter);
        _failedRequestCounter = Metric.CreateFailedRequestCounter(_meter);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();

        using var consoleMetricWriter = new ConsoleMetricWriter();

        var targets = new List<(RequestTarget target, string url)>
        {
            new (RequestTarget.Microsoft, "https://microsoft.com"),
            new (RequestTarget.GitHub, "https://github.com"),
            new (RequestTarget.LinkedIn, "https://linkedin.com"),
            new (RequestTarget.Invalid, "invalid_url"),
        };

        using var httpClient = new HttpClient();

        while (!cancellationToken.IsCancellationRequested)
        {
            var index = RandomNumberGenerator.GetInt32(targets.Count);
            var (target, targetUrl) = targets[index];

            try
            {
                Console.WriteLine($"{Environment.NewLine}Sending request to {targetUrl}...");

                // Record the 'sample.total_requests' counter metric.
                _totalRequestCounter.Add(1, target);

                using var response = await httpClient.GetAsync(targetUrl, cancellationToken).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                    // Record the 'sample.request_stats' histogram metric.
                    _requestsStatsHistogram.Record(
                        content.Length,
                        new RequestInfo
                        {
                            Target = target,
                            DayOfWeek = DateTimeOffset.UtcNow.DayOfWeek.ToString()
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

            await _timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public override void Dispose()
    {
        base.Dispose();

        _timer.Dispose();
        _meter.Dispose();
    }
}
