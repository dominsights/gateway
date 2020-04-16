using AutoFixture;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGatewayWorker;
using PaymentGatewayWorker.CQRS;
using PaymentGatewayWorker.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace PaymentGatewayWorkerUnitTests
{
    public class ProcessPaymentAppServiceTests
    {
        [Fact]
        public void ShouldStartProcessingPaymentWhenPaymentIsValid()
        {
            var logger = new Mock<ILogger<ProcessPaymentAppService>>();
            var mapper = new Mock<IMapper>();
            var paymentService = new Mock<PaymentService>();
            var mediator = new Mock<IMediator>();

            var fixture = new Fixture();
            var paymentDto = fixture.Build<PaymentDto>().Create();
            var payment = fixture.Build<PaymentGatewayWorker.Domain.Payments.Payment>().Create();
            payment.IsValid();

            paymentService.Setup(p => p.ValidateToCreate(It.IsAny<PaymentGatewayWorker.Domain.Payments.Payment>())).Returns(payment);

            var processPaymentAppService = new ProcessPaymentAppService(logger.Object, mapper.Object, paymentService.Object, mediator.Object);

            processPaymentAppService.ProcessPayments(paymentDto);

            mediator.Verify(m => m.Send(It.IsAny<AddNewPaymentCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void ShouldNotStartProcessingPaymentWhenPaymentIsInvalid()
        {
            var logger = new Mock<ILogger<ProcessPaymentAppService>>();
            var mapper = new Mock<IMapper>();
            var paymentService = new Mock<PaymentService>();
            var mediator = new Mock<IMediator>();

            var fixture = new Fixture();
            var paymentDto = fixture.Build<PaymentDto>().Create();
            var payment = fixture.Build<PaymentGatewayWorker.Domain.Payments.Payment>().Create();
            payment.CardNumber = null;
            payment.IsValid();

            paymentService.Setup(p => p.ValidateToCreate(It.IsAny<PaymentGatewayWorker.Domain.Payments.Payment>())).Returns(payment);

            var processPaymentAppService = new ProcessPaymentAppService(logger.Object, mapper.Object, paymentService.Object, mediator.Object);

            Assert.Throws<InvalidOperationException>(() => processPaymentAppService.ProcessPayments(paymentDto));

            mediator.Verify(m => m.Send(It.IsAny<AddNewPaymentCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
