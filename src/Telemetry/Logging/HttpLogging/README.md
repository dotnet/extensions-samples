# HTTP Logging enrichment and redaction

This sample shows how to extend [ASP.NET Core HTTP logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging)
with [enrichment and redaction capabilities](https://github.com/dotnet/extensions/blob/main/src/Libraries/Microsoft.AspNetCore.Diagnostics.Middleware/README.md#http-request-logs-enrichment-and-redaction).
Enrichment allows to add additional information to the log messages, while redaction
allows to remove sensitive information from the log messages.

The sample exposes a couple of HTTP URLs that demonstrate how extended HTTP logging functionality works:

* HTTP routes

    * `/api/chats/{chatId}`
    * `/api/chats/{chatId}/members/{memberId}`
    * `/api/chats/{chatId}/messages/{messageId}`
    * `/api/chats/{chatId}/attachments/{attachmentId}`

    demonstrate how to redact values of compliance sensitive HTTP route parameters. Also, when you navigate to each of these HTTP URLs,
    the log message information is enriched with additional data.

* HTTP URL `/home/index` demonstrates how to exclude certain HTTP paths from being logged.

Navigate to each of these HTTP URLs and see how extended HTTP logging functionality works.

For more information, see the source of the sample.

Useful links:

* [ASP.NET Core HTTP logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging)
* [Extended HTTP logging functionality](https://github.com/dotnet/extensions/blob/main/src/Libraries/Microsoft.AspNetCore.Diagnostics.Middleware/README.md#http-request-logs-enrichment-and-redaction)
* [Abstractions to classify compliance sensitive data](https://github.com/dotnet/extensions/blob/main/src/Libraries/Microsoft.Extensions.Compliance.Abstractions/README.md)
* [Implementation of a couple of data redaction algorithms](https://github.com/dotnet/extensions/blob/main/src/Libraries/Microsoft.Extensions.Compliance.Redaction/README.md)
