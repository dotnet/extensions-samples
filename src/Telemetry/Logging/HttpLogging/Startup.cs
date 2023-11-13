// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HttpLogging.Enrichment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.Logging;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Diagnostics;
using Microsoft.Net.Http.Headers;
using Shared.Compliance;

namespace HttpLogging;

internal sealed class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        _ = services
            .AddRouting()
            .AddControllers();

        // Here we register ASP.NET Core HTTP logging.
        _ = services.AddHttpLogging(_ => { });

        // Here we register HttpLogEnricher to enrich HTTP logs with additional information.
        _ = services.AddHttpLogEnricher<HttpLogEnricher>();

        // Here we register HTTP log redaction. Redaction is applied to HTTP route parameters,
        // request and response headers.
        _ = services.AddHttpLoggingRedaction(options =>
        {
            // Here we specify a strategy for logging HTTP request paths. "Formatted" means that
            // the path will contain actual values of route parameters.
            options.RequestPathLoggingMode = IncomingPathLoggingMode.Formatted;

            // Here we specify how to treat HTTP route parameters.
            // "Strict" means that all route parameters are considered as sensitive.
            options.RequestPathParameterRedactionMode = HttpRouteParameterRedactionMode.Strict;

            // Here we specify which HTTP paths we want to exclude from logging.
            options.ExcludePathStartsWith.Add("/home");

            // Here we specify data classification for HTTP route parameters:
            options.RouteParameterDataClasses.Add("chatId", DataTaxonomy.PrivateData);
            options.RouteParameterDataClasses.Add("messageId", DataTaxonomy.PublicData);

            // Data classification for request headers:
            options.RequestHeadersDataClasses.Add(HeaderNames.UserAgent, DataTaxonomy.PrivateData);
            options.RequestHeadersDataClasses.Add(HeaderNames.Accept, DataTaxonomy.PublicData);

            // Data classification for response headers:
            options.ResponseHeadersDataClasses.Add("SensitiveHeader", DataTaxonomy.PrivateData);
            options.ResponseHeadersDataClasses.Add(HeaderNames.ContentType, DataTaxonomy.PublicData);
        });

        // We need to register a redactor that will handle all sensitive data.
        // Here for the sake of showing redaction functionality we use "StarRedactor" and "NullRedactor" redactors.
        // In a real world scenario you would use the one that suits your needs (e.g. HMAC redactor).
        _ = services.AddRedaction(builder =>
        {
            _ = builder.SetRedactor<StarRedactor>(new DataClassificationSet(DataTaxonomy.PrivateData));
            _ = builder.SetRedactor<NullRedactor>(new DataClassificationSet(DataTaxonomy.PublicData));
        });
    }

    public static void Configure(IApplicationBuilder app)
    {
        _ = app.UseRouting();

        // Be aware that http request logging should be low in stack,
        // as different middlewares may feed HTTP request with data that you want to log.
        _ = app.UseHttpLogging();

        _ = app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapDefaultControllerRoute();
        });
    }
}
