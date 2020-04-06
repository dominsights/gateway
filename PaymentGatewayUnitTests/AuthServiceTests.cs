using Moq;
using PaymentGateway.Authorization;
using PaymentGateway.Authorization.Data;
using PaymentGateway.Authorization.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PaymentGatewayUnitTests
{
    public class AuthServiceTests
    {
        [Fact]
        public void ShouldSaveNewUser()
        {
            var jwtHandlerMock = new Mock<IJwtHandler>();
            var userAccountRepository = new Mock<IUserAccountRepository>();
            var passwordServiceMock = new Mock<IPasswordService>();
            passwordServiceMock.Setup(p => p.GenerateHashedPassword(It.IsAny<string>())).Returns(new Password());
            
            var authService = new AuthService(jwtHandlerMock.Object, userAccountRepository.Object, passwordServiceMock.Object);

            var userAccount = authService.SaveAsync("test", "test").Result;
            userAccountRepository.Verify(u => u.SaveAsync(It.IsAny<UserAccount>()), Times.Once);
            passwordServiceMock.Verify(p => p.GenerateHashedPassword(It.IsAny<string>()), Times.Once);
        }
    }
}
