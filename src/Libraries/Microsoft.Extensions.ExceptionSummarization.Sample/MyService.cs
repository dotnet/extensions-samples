// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.ExceptionSummarization.Sample;

internal sealed class MyService : IHostedService
{
    private readonly IExceptionSummarizer _summarizer;

    public MyService(IExceptionSummarizer summarizer)
    {
        // we get a summarizer from DI
        _summarizer = summarizer;
    }

#pragma warning disable CA1031 // Do not catch general exception types

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // here we do some complicated stuff which might throw an exception
        try
        {
            throw new ApplicationException("test", new WebException("test", WebExceptionStatus.RequestCanceled));
        }
        catch (Exception ex)
        {
            // get a summary of the exception
            var summary = _summarizer.Summarize(ex);

            Console.WriteLine($"Exception Type: {summary.ExceptionType}");
            Console.WriteLine($"Description   : {summary.Description}");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
