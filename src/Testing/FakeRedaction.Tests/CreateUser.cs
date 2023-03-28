// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.R9.Extensions.Data.Classification;
using Microsoft.R9.Extensions.Redaction;
using Microsoft.R9.Extensions.Time;

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

        internal IClock Clock { get; set; } = SystemClock.Instance;

        public User Handle(EUPI<string> username)
        {
            var user = new User(username, Clock.UtcNow);

            _logger.UserCreated(_provider, username);

            return user;
        }
    }
}