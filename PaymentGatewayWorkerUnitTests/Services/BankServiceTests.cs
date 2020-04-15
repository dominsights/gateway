using AutoFixture;
using Moq;
using PaymentGatewayWorker.Domain.Payments;
using PaymentGatewayWorker.Domain.Payments.Facades;
using PaymentGatewayWorker.Domain.Payments.Services;
using PaymentGatewayWorker.EventSourcing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGatewayWorkerUnitTests.Services
{
    public class BankServiceTests
    {
        [Fact]
        public void ShouldNotThrowExceptionWhenPaymentIsValidToSendToBank()
        {
            var eventRepository = new Mock<EventRepository>();
            eventRepository.Setup(e => e.IsNotApprovedOrDenied(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var bankApiClient = new Mock<BankApiClientFacade>();
            bankApiClient.Setup(b => b.SendPaymentToBankAsync(It.IsAny<Payment>())).ReturnsAsync(Guid.NewGuid());

            var bankService = new BankService(eventRepository.Object, bankApiClient.Object);

            var fixture = new Fixture();
            var payment = fixture.Build<Payment>().Create();
            bankService.SendPaymentForBankApprovalAsync(payment).Wait();
        }

        [Fact]
        public void ShouldThrowExceptionWhenPaymentDataIsInvalid()
        {
            var eventRepository = new Mock<EventRepository>();
            eventRepository.Setup(e => e.IsNotApprovedOrDenied(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            var bankApiClient = new Mock<BankApiClientFacade>();
            bankApiClient.Setup(b => b.SendPaymentToBankAsync(It.IsAny<Payment>())).ReturnsAsync(Guid.NewGuid());

            var bankService = new BankService(eventRepository.Object, bankApiClient.Object);

            var fixture = new Fixture();
            var payment = fixture.Build<Payment>().Create();
            payment.CardNumber = null;
            Assert.ThrowsAny<Exception>(() => bankService.SendPaymentForBankApprovalAsync(payment).Wait());
        }

        [Fact]
        public void ShouldThrowExceptionWhenPaymentIsAlreadySentToBank()
        {
            var eventRepository = new Mock<EventRepository>();
            eventRepository.Setup(e => e.IsNotApprovedOrDenied(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            var bankApiClient = new Mock<BankApiClientFacade>();
            bankApiClient.Setup(b => b.SendPaymentToBankAsync(It.IsAny<Payment>())).ReturnsAsync(Guid.NewGuid());

            var bankService = new BankService(eventRepository.Object, bankApiClient.Object);

            var fixture = new Fixture();
            var payment = fixture.Build<Payment>().Create();
            Assert.ThrowsAny<Exception>(() => bankService.SendPaymentForBankApprovalAsync(payment).Wait());
        }
    }
}
