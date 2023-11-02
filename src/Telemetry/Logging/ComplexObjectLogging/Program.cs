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
        // This will let us see the structure going to the logger
        using var loggerFactory =
            LoggerFactory.Create(builder =>
            {
                builder.EnableRedaction();
                builder.AddJsonConsole(o =>
                    o.JsonWriterOptions = new JsonWriterOptions
                    {
                        Indented = true
                    });

                builder.Services.AddRedaction(x => x.SetRedactor<StarRedactor>(new DataClassificationSet(DataTaxonomy.Classification)));
            });

        var logger = loggerFactory.CreateLogger("MyDemoLogger");

        Log.StartingUp(logger, new StartupAttributes(Environment.ProcessId, nameof(ComplexObjectLogging)));

        // Using as an extension method:
        logger.DataFrameSent(new DataFrame(nameof(Program).Length, byte.MinValue, int.MaxValue));

        // Logging sensitive data with redaction:
        Log.UserIdAvailabilityChanged(logger, "John Doe", new UserAvailability("SensitiveUsername", "Online"));
    }
}
