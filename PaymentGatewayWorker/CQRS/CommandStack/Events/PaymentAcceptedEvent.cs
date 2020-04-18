using MediatR;
using MongoDbRepository;
using PaymentGatewayWorker.Domain.Payments;
using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.CQRS.CommandStack.Events
{
    class PaymentAcceptedEvent : Event, INotification
    {
        public PaymentAcceptedEvent(Guid paymentId)
            : base()
        {
            AggregateId = paymentId;
        }
    }
}
