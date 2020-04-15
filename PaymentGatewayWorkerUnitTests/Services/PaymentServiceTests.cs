using AutoFixture;
using Moq;
using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using PaymentGatewayWorker.Domain.Payments.Data.Repository;
using PaymentGatewayWorker.Domain.Payments.Services;
using PaymentGatewayWorker.Domain.Services;
using PaymentGatewayWorker.EventSourcing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayWorkerUnitTests.Services
{
    public class PaymentServiceTests
    {
        [Fact]
        public void ShouldReturnIsValidWhenPaymentIsValid()
        {
            var paymentRepository = new Mock<PaymentRepository>();
            var eventRepository = new Mock<EventRepository>();

            paymentRepository.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult((PaymentGatewayWorker.Domain.Payments.Payment)null));

            paymentRepository.Setup(p => p.GetByFilterAsync(It.IsAny<PaymentFilter>())).Returns(Task.FromResult((PaymentGatewayWorker.Domain.Payments.Payment)null));

            var fixture = new Fixture();

            var payment = fixture.Build<PaymentGatewayWorker.Domain.Payments.Payment>().Create();

            var paymentService = new PaymentService(paymentRepository.Object, eventRepository.Object);

            var result = paymentService.ValidateToCreate(payment);

            Assert.True(result.ValidationResult.IsValid);
            Assert.True(result.IsValid());
        }

        [Fact]
        public void ShouldReturnInvalidWhenPaymentIsInvalid()
        {
            var paymentRepository = new Mock<PaymentRepository>();
            var eventRepository = new Mock<EventRepository>();

            paymentRepository.Setup(p => p.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new PaymentGatewayWorker.Domain.Payments.Payment()));

            paymentRepository.Setup(p => p.GetByFilterAsync(It.IsAny<PaymentFilter>())).Returns(Task.FromResult(new PaymentGatewayWorker.Domain.Payments.Payment()));

            var fixture = new Fixture();

            var payment = fixture.Build<PaymentGatewayWorker.Domain.Payments.Payment>().Create();
            payment.CardNumber = null;

            var paymentService = new PaymentService(paymentRepository.Object, eventRepository.Object);

            var result = paymentService.ValidateToCreate(payment);

            Assert.False(result.ValidationResult.IsValid);
            Assert.False(result.IsValid());
        }

        [Fact]
        public void ShouldReturnIsValidWhenPaymentIsValidToUpdate()
        {
            var paymentRepository = new Mock<PaymentRepository>();
            var eventRepository = new Mock<EventRepository>();

            var fixture = new Fixture();
            var payment = fixture.Build<PaymentGatewayWorker.Domain.Payments.Payment>().Create();
            paymentRepository.Setup(p => p.GetByBankResponseIdAsync(It.IsAny<Guid>())).ReturnsAsync(payment);
            eventRepository.Setup(e => e.IsNotApprovedOrDenied(It.IsAny<Guid>())).Returns(Task.FromResult(true));


            var response = fixture.Build<PaymentHubResponse>().Create();

            var paymentService = new PaymentService(paymentRepository.Object, eventRepository.Object);

            var result = paymentService.ValidateToUpdateStatusAsync(response).Result;

            Assert.True(result.ValidationResult.IsValid);
            Assert.True(result.IsValid());
        }

        [Fact]
        public void ShouldNotReturnIsValidWhenPaymentIsInvalidToUpdate()
        {
            var paymentRepository = new Mock<PaymentRepository>();
            var eventRepository = new Mock<EventRepository>();

            var fixture = new Fixture();
            var payment = fixture.Build<PaymentGatewayWorker.Domain.Payments.Payment>().Create();
            payment.CardNumber = null;
            paymentRepository.Setup(p => p.GetByBankResponseIdAsync(It.IsAny<Guid>())).ReturnsAsync(payment);
            eventRepository.Setup(e => e.IsNotApprovedOrDenied(It.IsAny<Guid>())).Returns(Task.FromResult(false));


            var response = fixture.Build<PaymentHubResponse>().Create();

            var paymentService = new PaymentService(paymentRepository.Object, eventRepository.Object);

            var result = paymentService.ValidateToUpdateStatusAsync(response).Result;

            Assert.False(result.ValidationResult.IsValid);
            Assert.False(result.IsValid());
        }
    }
}
