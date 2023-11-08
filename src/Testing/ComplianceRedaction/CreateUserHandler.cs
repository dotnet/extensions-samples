// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.Logging;

namespace ComplianceTesting
{
    internal sealed class CreateUserHandler
    {
        private readonly ILogger _logger;

        public CreateUserHandler(ILogger<CreateUserHandler> logger)
        {
            _logger = logger;
        }

        internal TimeProvider Clock { get; set; } = TimeProvider.System;

        public User Handle(string username)
        {
            var user = new User(username, Clock.GetUtcNow());

            _logger.UserCreated(username);

            return user;
        }
    }
}
