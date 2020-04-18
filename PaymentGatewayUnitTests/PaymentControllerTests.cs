using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Payments.Controllers;
using PaymentGateway.Payments.Models;
using PaymentGateway.Payments.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoFixture;

namespace PaymentGatewayUnitTests
{
    public class PaymentControllerTests : IClassFixture<PaymentGatewayFixture>
    {
        private PaymentGatewayFixture _fixture;

        [Fact]
        public void ShouldReturnAcceptedWhenPaymentIsSuccess()
        {
            var paymentServiceMock = new Mock<PaymentService>();

            paymentServiceMock.Setup(p => p.ProcessPaymentAsync(_fixture.Mapper.Map<PaymentDto>(_fixture.PaymentInput))).Returns(Task.FromResult(new Guid()));
            var paymentController = new PaymentController(paymentServiceMock.Object, new Mock<ILogger<PaymentController>>().Object, _fixture.Mapper);
            paymentController.ControllerContext = _fixture.ControllerContext;

            paymentController.Post(_fixture.PaymentInput).Wait();

            paymentServiceMock.Verify(m => m.ProcessPaymentAsync(It.IsAny<PaymentDto>()), Times.Once);
        }

        [Fact]
        public void ShouldReturnOkStatusWhenPaymentExists()
        {
            var fixture = new Fixture();
            var paymentDetailsMock = fixture.Build<PaymentDetailsDto>().Create();

            var paymentServiceMock = new Mock<PaymentService>();
            paymentServiceMock.Setup(p => p.GetPaymentDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(paymentDetailsMock);

            var paymentController = new PaymentController(paymentServiceMock.Object, new Mock<ILogger<PaymentController>>().Object, _fixture.Mapper);
            paymentController.ControllerContext = _fixture.ControllerContext;

            var response = paymentController.Get(Guid.NewGuid()).Result;
            var okResult = response as OkObjectResult;

            paymentServiceMock.Verify(p => p.GetPaymentDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void ShouldReturnBadRequestWhenPaymentDoesntExist()
        {
            var fixture = new Fixture();

            var paymentServiceMock = new Mock<PaymentService>();

            var paymentController = new PaymentController(paymentServiceMock.Object, new Mock<ILogger<PaymentController>>().Object, _fixture.Mapper);
            paymentController.ControllerContext = _fixture.ControllerContext;

            var response = paymentController.Get(Guid.NewGuid()).Result;
            var badRequestResult = response as BadRequestObjectResult;

            paymentServiceMock.Verify(p => p.GetPaymentDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void ShouldReturnBadRequestWhenError()
        {
            var fixture = new Fixture();

            var paymentServiceMock = new Mock<PaymentService>();
            paymentServiceMock.Setup(p => p.GetPaymentDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ThrowsAsync(new Exception());

            var paymentController = new PaymentController(paymentServiceMock.Object, new Mock<ILogger<PaymentController>>().Object, _fixture.Mapper);
            paymentController.ControllerContext = _fixture.ControllerContext;

            var response = paymentController.Get(Guid.NewGuid()).Result;
            var badRequestResult = response as BadRequestResult;

            paymentServiceMock.Verify(p => p.GetPaymentDetailsAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        public PaymentControllerTests(PaymentGatewayFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
