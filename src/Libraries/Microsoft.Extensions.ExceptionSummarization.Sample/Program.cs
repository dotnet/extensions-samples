// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.ExceptionSummarization.Sample;

internal static class Program
{
    internal static async Task Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder(args)
            //.CreateBuilder(args, x => x.LoggingEnabled = true)
            .ConfigureServices((_, services) =>
                {
                    services.AddExceptionSummarizer(b => b.AddHttpProvider());
                    services.AddHostedService<MyService>();
                })
            .Build();

        await host.RunAsync().ConfigureAwait(false);
    }
}
