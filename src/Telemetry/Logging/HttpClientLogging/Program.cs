// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;
using System.Net.Http;
using Microsoft.Extensions.Http.Diagnostics;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Compliance.Redaction;
using System.Net.Mime;
using Shared.Compliance;

var builder = Host.CreateApplicationBuilder(args);

// We add JSON console logging to see all tags in the log output:
builder.Logging.AddJsonConsole(x =>
    x.JsonWriterOptions = new() { Indented = true });

// We need to register a redactor that will handle all sensitive data.
// Here for the sake of showing redaction functionality we use "StarRedactor" and "NullRedactor" redactors.
// In a real world scenario you would use the one that suits your needs (e.g. HMAC redactor).
builder.Services.AddRedaction(x =>
{
    // We don't redact values that aren't sensitive:
    x.SetRedactor<NullRedactor>(DataTaxonomy.PublicData);

    // All sensitive data gets replaced with asterisks:
    x.SetRedactor<StarRedactor>(DataTaxonomy.PrivateData);
});

// We add the HttpClient logging and load its options from configuration.
// You can modify the configuration in appsettings.json to change the logging behavior.
builder.Services.AddExtendedHttpClientLogging(builder.Configuration.GetSection("HttpClientLogging"));

// It's possible to configure logging for a named or typed HttpClient:
var httpClientBuilder = builder.Services.AddHttpClient("MyNamedClient");

// We add the logging to the named HttpClient and configure it via action:
httpClientBuilder.AddExtendedHttpClientLogging(options =>
{
    // In Formatted mode all request path parameters are logged as part of HTTP URL path.
    // If you use Structured mode they will be logged as separate tags and the HTTP URL will contain the HTTP request route.
    options.RequestPathLoggingMode = OutgoingPathLoggingMode.Formatted;
    options.RouteParameterDataClasses.Add("userId", DataTaxonomy.PrivateData);
    options.RequestPathParameterRedactionMode = HttpRouteParameterRedactionMode.Loose;

    // We can also configure the logging for specific request and response headers:
    options.RequestHeadersDataClasses.Add("Accept", DataTaxonomy.PublicData);
    options.ResponseHeadersDataClasses.Add("Server", DataTaxonomy.PublicData);

    // If you want to log content headers, you need to enable the corresponding option.
    // The API is experimental, thus you need to explicitly acknowledge that.
#pragma warning disable EXTEXP0003
    options.LogContentHeaders = true;
    options.ResponseHeadersDataClasses.Add("Content-Length", DataTaxonomy.PublicData);
#pragma warning restore EXTEXP0003
});

// We can also add a custom log enricher to augment all HttpClient logs:
builder.Services.AddHttpClientLogEnricher<CustomLogEnricher>();

var host = builder.Build();

var httpClientFactory = host.Services.GetRequiredService<IHttpClientFactory>();

// We send the same request twice (first with non-named HttpClient and then with named one)
// to see how different options affect the logging:
using (var httpClient = httpClientFactory.CreateClient())
{
    await SendRequestAsync(httpClient).ConfigureAwait(false);
}

using (var httpClient = httpClientFactory.CreateClient("MyNamedClient"))
{
    await SendRequestAsync(httpClient).ConfigureAwait(false);
}

static async Task SendRequestAsync(HttpClient httpClient)
{
    using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/users/aspnet/repos");
    requestMessage.Headers.Accept.Add(new(MediaTypeNames.Application.Json));
    requestMessage.Headers.UserAgent.Add(new("HttpClientLoggingSample", productVersion: null));

    var requestMetadata = new RequestMetadata()
    {
        RequestRoute = "users/{userId}/repos",
        DependencyName = "GitHub",
        RequestName = "Get user repos"
    };

    requestMessage.SetRequestMetadata(requestMetadata);
    using var response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
}
