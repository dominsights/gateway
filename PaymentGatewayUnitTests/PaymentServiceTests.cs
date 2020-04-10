using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Payments.Services;
using RabbitMq.Infrastructure;
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
            var paymentService = new PaymentService(messagingMock.Object, new Mock<ILogger<PaymentService>>().Object);

            var dto = _fixture.Mapper.Map<PaymentDto>(_fixture.PaymentInput);
            dto.UserId = Guid.NewGuid();

            var id = paymentService.ProcessPaymentAsync(dto).Result;

            Assert.False(id == Guid.Empty);
            Assert.NotEqual(id, new Guid());

            string serializedPayment = JsonSerializer.Serialize(dto);
            messagingMock.Verify(m => m.SendPaymentAsync(serializedPayment), Times.Once);
        }

        public PaymentServiceTests(PaymentGatewayFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
