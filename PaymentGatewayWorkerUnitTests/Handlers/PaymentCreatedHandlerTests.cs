using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using PaymentGatewayWorker.CQRS.CommandStack.Handlers;
using PaymentGatewayWorker.Domain.Payments;
using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using PaymentGatewayWorker.Domain.Payments.Data.Repository;
using PaymentGatewayWorker.Domain.Payments.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace PaymentGatewayWorkerUnitTests.Handlers
{
    public class PaymentCreatedHandlerTests
    {
        [Fact]
        public void ShouldCreatePayment()
        {
            var bankResponseRepository = new Mock<BankResponseRepository>();
            var mediator = new Mock<IMediator>();
            var logger = new Mock<ILogger<PaymentCreatedHandler>>();
            var bankService = new Mock<BankService>();

            var handler = new PaymentCreatedHandler(bankResponseRepository.Object, mediator.Object, logger.Object, bankService.Object);

            var fixture = new Fixture();

            var @event = fixture.Build<PaymentCreatedEvent>()
                            .Create();

            handler.Handle(@event, new CancellationToken()).Wait();

            bankService.Verify(b => b.SendPaymentForBankApprovalAsync(It.IsAny<PaymentGatewayWorker.Domain.Payments.Payment>()), Times.Once);
            bankResponseRepository.Verify(b => b.SaveBankResponseAsync(It.IsAny<BankResponse>()), Times.Once);
            mediator.Verify(m => m.Publish(It.IsAny<PaymentSentForBankApprovalEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void ShouldNotCreatePayment()
        {
            var bankResponseRepository = new Mock<BankResponseRepository>();
            var mediator = new Mock<IMediator>();
            var logger = new Mock<ILogger<PaymentCreatedHandler>>();
            var bankService = new Mock<BankService>();
            bankService.Setup(b => b.SendPaymentForBankApprovalAsync(It.IsAny<PaymentGatewayWorker.Domain.Payments.Payment>())).Throws(new Exception());

            var handler = new PaymentCreatedHandler(bankResponseRepository.Object, mediator.Object, logger.Object, bankService.Object);

            var fixture = new Fixture();

            var @event = fixture.Build<PaymentCreatedEvent>()
                            .Create();

            handler.Handle(@event, new CancellationToken()).Wait();

            bankService.Verify(b => b.SendPaymentForBankApprovalAsync(It.IsAny<PaymentGatewayWorker.Domain.Payments.Payment>()), Times.Once);
            bankResponseRepository.Verify(b => b.SaveBankResponseAsync(It.IsAny<BankResponse>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<SendPaymentForBankApprovalErrorEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
