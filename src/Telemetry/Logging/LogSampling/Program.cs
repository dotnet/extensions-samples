// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LogSampling;

internal static class Program
{
    public static void Main()
    {
        var hostBuilder = Host.CreateApplicationBuilder();
        hostBuilder.Logging.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.TimestampFormat = "hh:mm:ss";
        });

        // Add the Random probabilistic sampler to the logging pipeline.
        hostBuilder.Logging.AddRandomProbabilisticSampler(hostBuilder.Configuration);

        var app = hostBuilder.Build();
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("SamplingDemo");

        // Simulate a prod application with many log messages generated:
        while (true)
        {
            Log.ErrorMessage(logger);

            for (int i = 0; i < 10; i++)
            {
                Log.InformationMessage(logger);
                Thread.Sleep(300);
            }
        }
    }
}
