using CQRS;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using PaymentGatewayWorker.Domain;
using PaymentGatewayWorker.Domain.Payments.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.CQRS.CommandStack.Sagas
{
    public class AddPaymentSaga : Saga,
        IStartWithMessage<AddNewPaymentCommand>
    {
        private IRepository _respository;
        private IBus _bus;

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

            var createdEvent = new AddNewPaymentSuccessEvent(request.Id, request);
            await _bus.RaiseEventAsync(createdEvent);
        }

        public AddPaymentSaga(IBus bus, IEventStore eventStore, IRepository repository)
            : base(bus, eventStore)
        {
            _respository = repository;
            _bus = bus;
        }

        //public AddPaymentSaga(IBus bus, IEventStore eventStore, IRepository repository)
        //    : base(bus, eventStore)
        //{

        //}
    }
}
