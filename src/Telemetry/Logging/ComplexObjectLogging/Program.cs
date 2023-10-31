// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ComplexObjectLogging;

internal static class Program
{
    public static void Main()
    {
        // This will let us see the structure going to the logger
        using var loggerFactory =
            LoggerFactory.Create(builder =>
                builder.AddJsonConsole(o =>
                    o.JsonWriterOptions = new JsonWriterOptions
                    {
                        Indented = true
                    }));

        var logger = loggerFactory.CreateLogger("MyDemoLogger");

        Log.StartingUp(logger, new StartupAttributes(Environment.ProcessId, nameof(ComplexObjectLogging)));
    }
}
