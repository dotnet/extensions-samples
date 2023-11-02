// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text.Json;
using ComplexObjectLogging.Compliance;
using ComplexObjectLogging.Models;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ComplexObjectLogging;

internal static class Program
{
    public static void Main()
    {
        // Here we construct the logger factory out of thin air,
        // but in a real world scenario you would use the one provided by the DI container.
        using var loggerFactory =
            LoggerFactory.Create(builder =>
            {
                // You can adjust the minimum level to see more or less information in the console output:
                builder.SetMinimumLevel(LogLevel.Debug);

                // We enable redaction support to be able to redact sensitive data:
                builder.EnableRedaction();

                // Here we use the JSON console logger to see the structure of the log records:
                builder.AddJsonConsole(o =>
                    o.JsonWriterOptions = new JsonWriterOptions
                    {
                        Indented = true
                    });

                // We need to register the redactor to be able to redact sensitive data:
                builder.Services.AddRedaction(x => x.SetRedactor<StarRedactor>(new DataClassificationSet(DataTaxonomy.Classification)));
            });

        var logger = loggerFactory.CreateLogger("MyDemoLogger");

        Log.StartingUp(logger, new StartupAttributes(Environment.ProcessId, nameof(ComplexObjectLogging)));

        Log.DataFrameSent(logger, new DataFrame(nameof(Program).Length, byte.MinValue, int.MaxValue));

        // Here we log an enumerable:
        Log.DataFrameSent(logger, ["destination_1", "destination_2"]);

        // This time it does nothing since the logger is null:
        Log.DataFrameSent(null, ["destination_1", "destination_2"]);

        // Logging sensitive data with redaction:
        Log.UserIdAvailabilityChanged(logger, "John Doe", new UserAvailability("SensitiveUsername", "Online"));
    }
}
