using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Authorization;
using PaymentGateway.Authorization.Controllers;
using PaymentGateway.Authorization.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayUnitTests
{
    public class AuthControllerTests
    {
        [Fact]
        public void ShouldLoginSuccesfully()
        {
            var authServiceMock = new Mock<AuthService>();
            authServiceMock.Setup(a => a.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(new UserJwt()));
        
            var authController = new AuthController(authServiceMock.Object, new Mock<ILogger<AuthController>>().Object);

            var response = authController.Login(new Login()).Result;
            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void ShouldNotLoginWhereThereIsNoUser()
        {
            var authServiceMock = new Mock<AuthService>();

            var authController = new AuthController(authServiceMock.Object, new Mock<ILogger<AuthController>>().Object);

            var response = authController.Login(new Login()).Result;
            var badRequestResult = response as BadRequestObjectResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void ShouldRegisterNewUser()
        {
            var authServiceMock = new Mock<AuthService>();

            var authController = new AuthController(authServiceMock.Object, new Mock<ILogger<AuthController>>().Object);

            var response = authController.Register(new RegisterModel() { UserName = "test", Password = "test" }).Result;
            var createdResult = response as CreatedAtActionResult;

            authServiceMock.Verify(a => a.SaveAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public void ShouldReturnBadRequestWhenErrorWhileRegisteringNewUser()
        {
            var authServiceMock = new Mock<AuthService>();
            authServiceMock.Setup(a => a.SaveAsync(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            var authController = new AuthController(authServiceMock.Object, new Mock<ILogger<AuthController>>().Object);

            var response = authController.Register(new RegisterModel() { UserName = "test", Password = "test" }).Result;

            var badRequestResult = response as BadRequestObjectResult;

            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}
