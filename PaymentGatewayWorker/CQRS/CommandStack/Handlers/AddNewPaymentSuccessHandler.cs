using CQRS;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.CQRS.CommandStack.Handlers
{
    public class AddNewPaymentSuccessHandler : IHandleMessage<PaymentCreatedEvent>
    {
        private IBus _bus;
        private IEventStore _eventStore;

        public Task HandleAsync(PaymentCreatedEvent message)
        {
            // Do something when success
            throw new NotImplementedException();
        }

        public AddNewPaymentSuccessHandler(IBus bus, IEventStore eventStore)
        {
            _bus = bus;
            _eventStore = eventStore;
        }
    }
}
