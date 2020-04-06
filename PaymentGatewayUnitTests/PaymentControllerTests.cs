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

namespace PaymentGatewayUnitTests
{
    public class PaymentControllerTests : IClassFixture<PaymentGatewayFixture>
    {
        private PaymentGatewayFixture _fixture;

        [Fact]
        public void ShouldReturnAcceptedWhenPaymentIsSuccess()
        {
            var paymentServiceMock = new Mock<IPaymentService>();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, new Guid().ToString())
            };

            var identity = new ClaimsIdentity(claims, "test");

            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new System.Security.Claims.ClaimsPrincipal(identity)
                }
            };

            paymentServiceMock.Setup(p => p.ProcessPaymentAsync(_fixture.Mapper.Map<PaymentDto>(_fixture.PaymentInput))).Returns(Task.FromResult(new Guid()));
            var paymentController = new PaymentController(paymentServiceMock.Object, new Mock<ILogger<PaymentController>>().Object, _fixture.Mapper);
            paymentController.ControllerContext = context;

            paymentController.Post(_fixture.PaymentInput).Wait();

            paymentServiceMock.Verify(m => m.ProcessPaymentAsync(It.IsAny<PaymentDto>()), Times.Once);
        }

        public PaymentControllerTests(PaymentGatewayFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
