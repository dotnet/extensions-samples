// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RequestCheckpoint.Controllers;

public class HomeController : Controller
{
    private readonly TimeSpan _secondsDelay = TimeSpan.FromSeconds(1);
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation($"{0} is here and let's wait just {1} seconds", nameof(HomeController), _secondsDelay.TotalSeconds);

        await Task.Delay(_secondsDelay, HttpContext.RequestAborted).ConfigureAwait(false);

        return View();
    }
}
