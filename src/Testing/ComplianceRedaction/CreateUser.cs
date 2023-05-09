// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Logging;

namespace FakeRedaction
{
    internal sealed class CreateUser
    {
        private readonly ILogger _logger;
        private readonly IRedactorProvider _provider;

        public CreateUser(IRedactorProvider redactorProvider, ILogger<CreateUser> logger)
        {
            _provider = redactorProvider;
            _logger = logger;
        }

        internal TimeProvider Clock { get; set; } = TimeProvider.System;

        public User Handle(string username)
        {
            var user = new User(username, Clock.GetUtcNow());

            _logger.UserCreated(_provider, username);

            return user;
        }
    }
}
