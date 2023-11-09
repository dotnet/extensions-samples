// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HttpLogging;

internal static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureLogging(builder =>
            {
                _ = builder.ClearProviders();

                _ = builder
                    .AddJsonConsole(options =>
                    {
                        options.JsonWriterOptions = new JsonWriterOptions
                        {
                            Indented = true
                        };
                    });
            })
            .ConfigureWebHostDefaults(builder =>
            {
                _ = builder.UseStartup<Startup>();
            });
    }
}
