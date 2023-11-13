// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;

namespace HttpLogging.Controllers;

[ApiController]
[Route("home")]
public class HomeController : ControllerBase
{
    // HTTP requests to /home/index will not be logged, because it is excluded from logging
    // using LoggingRedactionOptions.ExcludePathStartsWith property. See Startup.cs.
    [HttpGet("index")]
    public string Index()
    {
        return "Learn about dotnet/extensions: https://github.com/dotnet/extensions";
    }
}
