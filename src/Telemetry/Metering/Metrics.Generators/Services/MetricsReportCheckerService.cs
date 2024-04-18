// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Metrics.Generators.Services;

internal sealed class MetricsReportCheckerService : IHostedService
{
    // If you changed the "<MetricsReportOutputPath>" property in .csproj file,
    // you need to update the value below:
    private const string ReportLocation = "./";
    private const string MetricsReportFileName = "MetricsReport.json";

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var fileName = Path.Combine(ReportLocation, MetricsReportFileName);

        return CheckReport(fileName);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static Task CheckReport(string fileName)
    {
        var reportsLocation = Path.GetFullPath(fileName);
        if (File.Exists(reportsLocation))
        {
            Console.WriteLine("[Success] Metrics report is generated in: " + reportsLocation);
            Console.WriteLine("Its content is listed below:");
            Console.WriteLine("{0}", File.ReadAllText(reportsLocation));

            return Task.CompletedTask;
        }
        else
        {
            Console.Error.WriteLine("[Error] No report generated in: " + reportsLocation);

            return Task.FromException(new FileNotFoundException("No metrics report generated", reportsLocation));
        }
    }
}
