using Moq;
using PaymentGateway.Authorization;
using PaymentGateway.Authorization.Data;
using PaymentGateway.Authorization.Services;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayUnitTests
{
    public class AuthServiceTests
    {
        [Fact]
        public void ShouldSaveNewUser()
        {
            var jwtHandlerMock = new Mock<JwtHandler>();
            var userAccountRepository = new Mock<UserAccountRepository>();
            var passwordServiceMock = new Mock<PasswordService>();
            passwordServiceMock.Setup(p => p.GenerateHashedPassword(It.IsAny<string>())).Returns(new Password());
            
            var authService = new AuthService(jwtHandlerMock.Object, userAccountRepository.Object, passwordServiceMock.Object);

            var userAccount = authService.SaveAsync("test", "test").Result;
            userAccountRepository.Verify(u => u.SaveAsync(It.IsAny<UserAccount>()), Times.Once);
            passwordServiceMock.Verify(p => p.GenerateHashedPassword(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldLoginSuccesfully()
        {
            var jwtHandlerMock = new Mock<JwtHandler>();
            var userAccountRepository = new Mock<UserAccountRepository>();
            var passwordServiceMock = new Mock<PasswordService>();

            jwtHandlerMock.Setup(j => j.Create(It.IsAny<string>())).Returns(new JWT());
            userAccountRepository.Setup(u => u.GetByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult(new UserAccount() { Id = new Guid() }));
            passwordServiceMock.Setup(p => p.IsPasswordValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var authService = new AuthService(jwtHandlerMock.Object, userAccountRepository.Object, passwordServiceMock.Object);
            var userJwt = authService.LoginAsync("test", "test").Result;

            Assert.NotNull(userJwt);
            userAccountRepository.Verify(u => u.GetByUsernameAsync(It.IsAny<string>()), Times.Once);
            jwtHandlerMock.Verify(u => u.Create(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldNotLoginWhenInvalidPassword()
        {
            var jwtHandlerMock = new Mock<JwtHandler>();
            var userAccountRepository = new Mock<UserAccountRepository>();
            var passwordServiceMock = new Mock<PasswordService>();

            userAccountRepository.Setup(u => u.GetByUsernameAsync(It.IsAny<string>())).Returns(Task.FromResult(new UserAccount() { Id = new Guid() }));
            passwordServiceMock.Setup(p => p.IsPasswordValid(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var authService = new AuthService(jwtHandlerMock.Object, userAccountRepository.Object, passwordServiceMock.Object);

            Assert.ThrowsAnyAsync<InvalidCredentialException>(() => authService.LoginAsync("test", "test"));
        }
    }
}
