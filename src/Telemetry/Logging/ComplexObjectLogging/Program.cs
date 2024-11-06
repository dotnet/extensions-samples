// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text.Json;
using ComplexObjectLogging.Models;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Compliance;

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

                // We need to register the redactor that will handle all sensitive data.
                // Here we use the "StarRedactor" which replaces the sensitive data with asterisks.
                // In a real world scenario you would use the one that suits your needs (e.g. HMAC redactor).
                builder.Services.AddRedaction(x => x.SetRedactor<StarRedactor>(new DataClassificationSet(DataTaxonomy.PrivateData)));
            });

        var logger = loggerFactory.CreateLogger("MyDemoLogger");
        
        Log.StartingUp(logger, new StartupAttributes(Environment.MachineName, nameof(ComplexObjectLogging)));

        Log.DataFrameSent(logger, new DataFrame(nameof(Program).Length, byte.MinValue, short.MaxValue));

        // Here we log an enumerable:
        Log.DataFrameSent(logger, ["destination_1", "destination_2"]);

        // This time it does nothing since the logger is null:
        Log.DataFrameSent(null, ["destination_1", "destination_2"]);

        // Logging sensitive data with redaction:
        Log.UserIdAvailabilityChanged(logger, "John Doe", new UserAvailability("SensitiveUsername", "Online"));

        // Logging properties transitively:
        Log.UserLoggedIn(logger, new User("abcd", "John Doe", "john.doe@contoso.com", new InnerUserData()));
    }
}
