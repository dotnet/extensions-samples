// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Compliance.Testing;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Testing;
using Xunit;

namespace ComplianceTesting
{
    public class CreateUserTest
    {
        [Fact]
        public void Dummy_Types_Can_Fill_Required_Dependency()
        {
            // We just fill dependencies to test business logic.
            var handler = new CreateUserHandler(NullLogger<CreateUserHandler>.Instance);
            const string Username = "Jan";

            var user = handler.Handle(Username);

            // We just assert business logic and don't care about logging and redaction.
            Assert.NotNull(user);
            Assert.Equal(Username, user.Name);
        }

        [Fact(Skip = "Broken")]
        public void Fake_Redactor_Allows_To_Check_If_Data_Got_Redacted()
        {
            var fakeProvider = new FakeRedactorProvider();
            var fakeLogger = new FakeLogger<CreateUserHandler>();

            var handler = new CreateUserHandler(fakeLogger);
            const string Username = "Jan";

            _ = handler.Handle(Username);

            // We focus on checking if redaction was performed correctly.
            Assert.Equal(Username, fakeProvider.Collector.LastRedactedData.Original);
            Assert.Single(fakeProvider.Collector.AllRedactedData);
        }
    }
}
