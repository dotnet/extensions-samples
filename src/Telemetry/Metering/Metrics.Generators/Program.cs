// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Metrics.Generators;

internal static class Startup
{
    public static void Main()
    {
        Task.Run(async () =>
        {
            using var telemetryEmitter = new TelemetryEmitter();
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

            while (true)
            {
                var index = random.Next(targets.Count);
                var target = targets[index].Item1;
                var targetUrl = targets[index].Item2;

                try
                {
                    Console.WriteLine($"Sending request to ${targetUrl}...");
                    telemetryEmitter.RecordRequest(target);

                    var response = await httpClient.GetAsync(targetUrl).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        telemetryEmitter.RecordRequestStats(
                            content.Length,
                            new RequestInfo
                            {
                                Target = target,
                                DayOfWeek = DateTimeOffset.UtcNow.DayOfWeek
                            });
                    }
                    else
                    {
                        telemetryEmitter.RecordFailedRequest(target, reason: response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    telemetryEmitter.RecordFailedRequest(target, reason: ex.GetType().Name);
                }

                Console.WriteLine();

                await Task.Delay(5000).ConfigureAwait(false);
            }
        });

        Console.ReadLine();
    }
}
