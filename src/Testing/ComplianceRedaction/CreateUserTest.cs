// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Compliance.Redaction;
using Microsoft.Extensions.Compliance.Redaction.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.R9.Extensions.Logging;
using Xunit;

namespace FakeRedaction
{
    public class CreateUserTest
    {
        [Fact]
        public void Dummy_Redactor_Provider_Can_Fill_Required_Dependency()
        {
            // We just fill dependencies to test business logic.
            var handler = new CreateUser(NullRedactorProvider.Instance, NullLogger<CreateUser>.Instance);
            const string Username = "Jan";

            var user = handler.Handle(Username);

            // We just assert business logic and don't care about logging and redaction.
            Assert.NotNull(user);
            Assert.Equal(Username, user.Name);
        }

        [Fact]
        public void Fake_Redactor_Allows_To_Check_If_Data_Got_Redacted()
        {
            var fakeProvider = new FakeRedactorProvider();
            var fakeLogger = new FakeLogger<CreateUser>();

            var handler = new CreateUser(fakeProvider, fakeLogger);
            const string Username = "Jan";
            const int ExpectedRedactedData = 1;

            _ = handler.Handle(Username);

            // We focus on checking if redaction was performed correctly.
            Assert.Equal(Username, fakeProvider.Collector.LastRedactedData.Original);
            Assert.Equal(ExpectedRedactedData, fakeProvider.Collector.AllRedactedData.Count);
        }
    }
}
