using MediatR;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using PaymentGatewayWorker.EventSourcing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.CQRS.CommandStack.Handlers
{
    public class AddNewPaymentSuccessHandler : INotificationHandler<PaymentCreatedEvent>
    {
        private IEventStore _eventStore;

        public Task HandleAsync(PaymentCreatedEvent message)
        {
            // Do something when success
            throw new NotImplementedException();
        }

        public Task Handle(PaymentCreatedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public AddNewPaymentSuccessHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
    }
}
