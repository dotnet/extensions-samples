// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TracingHttp;

[ApiController]
[Route("[controller]")]
public class ChatsController : Controller
{
    private readonly ITracingService _tracingService;

    public ChatsController(ITracingService tracingService)
    {
        _tracingService = tracingService;
    }

    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetChatById(string chatId)
    {
        var statusCode = await _tracingService.GetChatById(chatId).ConfigureAwait(false);

        return statusCode switch
        {
            HttpStatusCode.BadRequest => BadRequest(),
            HttpStatusCode.NotFound => NotFound(),
            HttpStatusCode.OK => Ok("Learn about R9: http://aka.ms/r9"),
            _ => Problem()
        };
    }

    [HttpGet("details/{chatId}")]
    public IActionResult Details(string chatId)
    {
        ViewData["Message"] = _tracingService.GetTracingServiceTags();
        return View();
    }
}
