// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HttpLogging.Compliance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HttpLogging.Controllers;

[ApiController]
[Route("api/chats")]
public class ChatController : ControllerBase
{
    // Data classification for "chatId" parameter will be taken from
    // LoggingRedactionOptions.RouteParameterDataClasses configured in Startup.cs.
    // "chatId" is private data, thus it's value will be redacted.
    [HttpGet("{chatId}")]
    public string GetChat(string chatId)
    {
        AddSensitiveHeader(Response);
        return $"Getting chat info...";
    }

    // You can use attributes to specify data classification for parameters.
    // "memberId" is private data, thus it's value will be redacted.
    [HttpGet("{chatId}/members/{memberId}")]
    public string GetChatMember(string chatId, [PrivateData] string memberId)
    {
        AddSensitiveHeader(Response);
        return "Getting chat member info...";
    }

    // Data classification for "messageId" is configured in Startup.cs.
    // "messageId" is public data, thus it's value will be logged as-is.
    [HttpGet("{chatId}/messages/{messageId}")]
    public string GetChatMessage(string chatId, string messageId)
    {
        AddSensitiveHeader(Response);
        return "Getting chat message info...";
    }

    // Data classification for "attachmentId" is not specified neither in Startup.cs
    // nor using data classification attributes.
    // If a route parameter doesn't have data classification, then the property
    // LoggingRedactionOptions.RequestPathParameterRedactionMode defines how
    // it will be redacted.
    [HttpGet("{chatId}/attachments/{attachmentId}")]
    public string GetChatAttachment(string chatId, string attachmentId)
    {
        AddSensitiveHeader(Response);
        return "Getting chat attachment info...";
    }

    // Here we add a sensitive header to the response to demonstrate how it will be redacted.
    private static void AddSensitiveHeader(HttpResponse response)
    {
        response.Headers.Append("SensitiveHeader", "Sensitive header value");
    }
}
