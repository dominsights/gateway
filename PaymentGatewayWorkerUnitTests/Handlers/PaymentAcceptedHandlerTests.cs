using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using PaymentGatewayWorker.CQRS.CommandStack.Handlers;
using RabbitMQService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace PaymentGatewayWorkerUnitTests.Handlers
{
    public class PaymentAcceptedHandlerTests
    {
        [Fact]
        public void ShouldHandleEvent()
        {
            var fixture = new Fixture();
            var notification = fixture.Build<PaymentAcceptedEvent>().Create();
            Mock<RabbitMqPublisher> rabbitMqPublisher = new Mock<RabbitMqPublisher>();
            var handler = new PaymentAcceptedHandler(rabbitMqPublisher.Object, new Mock<ILogger<PaymentAcceptedHandler>>().Object);
            handler.Handle(notification, new CancellationToken()).Wait();

            rabbitMqPublisher.Verify(r => r.SendMessageAsync(It.IsAny<string>(), "response_queue"), Times.Once);
        }

        [Fact]
        public void ShouldLogErrorWhenException()
        {
            var fixture = new Fixture();
            var notification = fixture.Build<PaymentAcceptedEvent>().Create();
            Mock<RabbitMqPublisher> rabbitMqPublisher = new Mock<RabbitMqPublisher>();
            rabbitMqPublisher.Setup(r => r.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception());
            Mock<ILogger<PaymentAcceptedHandler>> logger = new Mock<ILogger<PaymentAcceptedHandler>>();

            var handler = new PaymentAcceptedHandler(rabbitMqPublisher.Object, logger.Object);
            Assert.ThrowsAsync<Exception>(() => handler.Handle(notification, new CancellationToken()));

            // Nothing more to assert?
        }
    }
}
