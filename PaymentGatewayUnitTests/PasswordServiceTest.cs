using PaymentGateway.Authorization.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PaymentGatewayUnitTests
{
    public class PasswordServiceTest
    {
        [Fact]
        public void ShouldGenerateCorrectPasswordFromCorrectSalt()
        {
            var passwordService = new PasswordService();

            var hashedPassword = passwordService.GenerateHashedPassword("test");

            Assert.True(passwordService.IsPasswordValid("test", hashedPassword.Hash, hashedPassword.Salt));
        }

        [Fact]
        public void ShouldInvalidatePasswordWhenPasswordIsDifferent()
        {
            var passwordService = new PasswordService();

            var hashedPassword = passwordService.GenerateHashedPassword("test");

            Assert.False(passwordService.IsPasswordValid("test1", hashedPassword.Hash, hashedPassword.Salt));
        }

        [Fact]
        public void ShouldInvalidatePasswordWhenHashIsDifferent()
        {
            var passwordService = new PasswordService();

            var hashedPassword = passwordService.GenerateHashedPassword("test");

            Assert.False(passwordService.IsPasswordValid("test", hashedPassword.Hash, "test"));
        }
    }
}
