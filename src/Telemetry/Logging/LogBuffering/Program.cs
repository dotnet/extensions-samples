// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using LogBuffering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Buffering;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var hostBuilder = Host.CreateApplicationBuilder();

hostBuilder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "hh:mm:ss";
    options.UseUtcTimestamp = true;
});

// Add the Global buffer to the logging pipeline.
hostBuilder.Logging.AddGlobalBuffer(hostBuilder.Configuration);

using var app = hostBuilder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("BufferingDemo");
var buffer = app.Services.GetRequiredService<GlobalLogBuffer>();

var i = 1;
while(true)
{
    try
    {
        logger.InformationMessage();

        if(i % 10 == 0)
        {
           throw new Exception("Simulated exception");
        }
    }
    catch (Exception ex)
    {
        logger.ErrorMessage(ex.Message);
        buffer.Flush();
    }

    i++;
    await Task.Delay(1000).ConfigureAwait(false);
}
