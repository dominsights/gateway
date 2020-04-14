using CQRS;
using Microsoft.Extensions.Logging;
using PaymentGatewayWorker.CQRS.CommandStack.Commands;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using PaymentGatewayWorker.Domain;
using PaymentGatewayWorker.Domain.Payments.Data.Repository;
using PaymentGatewayWorker.Domain.Payments.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.CQRS.CommandStack.Sagas
{
    public class PaymentSaga : Saga,
        IStartWithMessage<AddNewPaymentCommand>,
        IHandleMessage<PaymentCreatedEvent>
    {
        private ILogger<PaymentSaga> _logger;
        private IRepository _respository;
        private IBus _bus;
        private BankService _bankService;

        public async Task HandleAsync(AddNewPaymentCommand message)
        {
            var request = Payment.Factory.NewPayment(message.AggregateId, message.UserId, message.CardNumber,
                message.ExpiryMonth, message.ExpiryYear, message.Amount, message.CurrencyCode, message.CVV);

            var response = await _respository.CreateFromRequestAsync(request);

            if (!response.Success)
            {
                var errorEvent = new AddNewPaymentErrorEvent();
                await _bus.RaiseEventAsync(errorEvent);
            }

            var createdEvent = new PaymentCreatedEvent(request.Id, request);
            await _bus.RaiseEventAsync(createdEvent);
        }

        public async Task HandleAsync(PaymentCreatedEvent message)
        {
            var command = new SendPaymentForBankApprovalCommand(message.AggregateId, message.Data.UserId, message.Data.CardNumber, message.Data.ExpiryMonth,
                message.Data.ExpiryMonth, message.Data.Amount, message.Data.CurrencyCode, message.Data.CVV);

            try
            {
                var response = await _bankService.SendPaymentForBankApprovalAsync(command);

                var sentToBankEvent = new PaymentSentForBankApprovalEvent(command.AggregateId, message.Data);
                await _bus.RaiseEventAsync(sentToBankEvent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to send payment to bank.");
                var errorEvent = new SendPaymentForBankApprovalErrorEvent();
                await _bus.RaiseEventAsync(errorEvent);
            }
        }

        public PaymentSaga(IBus bus, IEventStore eventStore, IRepository repository, BankService bankService, ILogger<PaymentSaga> logger)
            : base(bus, eventStore)
        {
            _logger = logger;
            _respository = repository;
            _bus = bus;
            _bankService = bankService;
        }

        //public AddPaymentSaga(IBus bus, IEventStore eventStore, IRepository repository)
        //    : base(bus, eventStore)
        //{

        //}
    }
}
