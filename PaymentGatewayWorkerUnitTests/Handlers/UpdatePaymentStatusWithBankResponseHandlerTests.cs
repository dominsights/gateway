using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGatewayWorker.CQRS.CommandStack.Commands;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using PaymentGatewayWorker.CQRS.CommandStack.Sagas;
using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using PaymentGatewayWorker.Domain.Payments.Data.Repository;
using PaymentGatewayWorker.Domain.Payments.Services;
using PaymentGatewayWorker.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayWorkerUnitTests.Handlers
{
    public class UpdatePaymentStatusWithBankResponseHandlerTests
    {
        [Fact]
        public void ShouldUpdatePaymentStatus()
        {
            var fixture = new Fixture();

            var mediator = new Mock<IMediator>();
            var logger = new Mock<ILogger<UpdatePaymentStatusWithBankResponseHandler>>();
            var paymentService = new Mock<PaymentService>();
            var paymentRepository = new Mock<PaymentRepository>();

            var payment = fixture.Build<PaymentGatewayWorker.Domain.Payments.Payment>().Create();
            payment.IsValid();

            paymentService.Setup(p => p.ValidateToUpdateStatusAsync(It.IsAny<PaymentHubResponse>())).Returns(Task.FromResult(payment));

            var handler = new UpdatePaymentStatusWithBankResponseHandler(mediator.Object, logger.Object, paymentService.Object, paymentRepository.Object);

            var @event = fixture.Build<UpdatePaymentStatusWithBankResponseCommand>().Create();
            handler.Handle(@event, new CancellationToken()).Wait();

            mediator.Verify(m => m.Publish(It.IsAny<PaymentAcceptedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
            paymentRepository.Verify(p => p.UpdatePaymentReadModelStatusAsync(It.IsAny<Guid>(), PaymentStatus.APPROVED), Times.Once);
        }

        [Fact]
        public void ShouldNotUpdatePaymentStatusWhenPaymentIsInvalid()
        {
            var fixture = new Fixture();

            var mediator = new Mock<IMediator>();
            var logger = new Mock<ILogger<UpdatePaymentStatusWithBankResponseHandler>>();
            var paymentService = new Mock<PaymentService>();
            var paymentRepository = new Mock<PaymentRepository>();

            var payment = fixture.Build<PaymentGatewayWorker.Domain.Payments.Payment>().Create();
            payment.CardNumber = null;
            payment.IsValid();

            paymentService.Setup(p => p.ValidateToUpdateStatusAsync(It.IsAny<PaymentHubResponse>())).Returns(Task.FromResult(payment));

            var handler = new UpdatePaymentStatusWithBankResponseHandler(mediator.Object, logger.Object, paymentService.Object, paymentRepository.Object);

            var @event = fixture.Build<UpdatePaymentStatusWithBankResponseCommand>().Create();
            handler.Handle(@event, new CancellationToken()).Wait();

            mediator.Verify(m => m.Publish(It.IsAny<PaymentAcceptedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
            paymentRepository.Verify(p => p.UpdatePaymentReadModelStatusAsync(It.IsAny<Guid>(), It.IsAny<PaymentStatus>()), Times.Never);
        }

        [Fact]
        public void ShouldNotUpdatePaymentStatusWhenException()
        {
            var fixture = new Fixture();

            var mediator = new Mock<IMediator>();
            var logger = new Mock<ILogger<UpdatePaymentStatusWithBankResponseHandler>>();
            var paymentRepository = new Mock<PaymentRepository>();

            var paymentService = new Mock<PaymentService>();
            paymentService.Setup(p => p.ValidateToUpdateStatusAsync(It.IsAny<PaymentHubResponse>())).ThrowsAsync(new Exception());

            var payment = fixture.Build<PaymentGatewayWorker.Domain.Payments.Payment>().Create();
            payment.CardNumber = null;
            payment.IsValid();

            var handler = new UpdatePaymentStatusWithBankResponseHandler(mediator.Object, logger.Object, paymentService.Object, paymentRepository.Object);

            var @event = fixture.Build<UpdatePaymentStatusWithBankResponseCommand>().Create();
            handler.Handle(@event, new CancellationToken()).Wait();

            mediator.Verify(m => m.Publish(It.IsAny<PaymentAcceptedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
            paymentRepository.Verify(p => p.UpdatePaymentReadModelStatusAsync(It.IsAny<Guid>(), It.IsAny<PaymentStatus>()), Times.Never);
        }
    }
}
