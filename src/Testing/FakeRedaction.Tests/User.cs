﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.R9.Extensions.Data.Classification;

namespace FakeRedaction
{
    /// <summary>
    /// A simple user type, which features a registration time.
    /// </summary>
    internal sealed class User
    {
        internal User(EUPI<string> name, DateTimeOffset registeredAt)
        {
            Name = name;
            RegisteredAt = registeredAt;
        }

        /// <summary>
        /// Gets the user's name.
        /// </summary>
        public EUPI<string> Name { get; }

        /// <summary>
        /// Gets time of user registration.
        /// </summary>
        public DateTimeOffset RegisteredAt { get; }
    }
}