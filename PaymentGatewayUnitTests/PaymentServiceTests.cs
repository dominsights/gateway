using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDbRepository;
using Moq;
using PaymentGateway.Payments.Models;
using PaymentGateway.Payments.Services;
using RabbitMQService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xunit;

namespace PaymentGatewayUnitTests
{
    public class PaymentServiceTests : IClassFixture<PaymentGatewayFixture>
    {
        private PaymentGatewayFixture _fixture;

        [Fact]
        public void ShouldCallMessagingService()
        {
            var messagingMock = new Mock<RabbitMqPublisher>();
            var paymentService = new PaymentService(messagingMock.Object, new Mock<ILogger<PaymentService>>().Object, new Mock<PaymentReadRepository>().Object, new Mock<IMapper>().Object);

            var dto = _fixture.Mapper.Map<PaymentDto>(_fixture.PaymentInput);
            dto.UserId = Guid.NewGuid();

            var id = paymentService.ProcessPaymentAsync(dto).Result;

            Assert.False(id == Guid.Empty);
            Assert.NotEqual(id, new Guid());

            string serializedPayment = JsonSerializer.Serialize(dto);
            messagingMock.Verify(m => m.SendMessageAsync(serializedPayment, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldReturnPaymentDetails()
        {
            Mock<PaymentReadRepository> paymentReadRepository = new Mock<PaymentReadRepository>();

            var productId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var paymentService = new PaymentService(new Mock<RabbitMqPublisher>().Object, new Mock<ILogger<PaymentService>>().Object, paymentReadRepository.Object, new Mock<IMapper>().Object);
            paymentService.GetPaymentDetailsAsync(productId, userId).Wait();

            paymentReadRepository.Verify(p => p.GetPaymentDetailsAsync(productId, userId), Times.Once);
        }

        public PaymentServiceTests(PaymentGatewayFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
