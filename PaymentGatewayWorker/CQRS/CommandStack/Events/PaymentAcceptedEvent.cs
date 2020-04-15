using MediatR;
using PaymentGatewayWorker.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.CQRS.CommandStack.Events
{
    class PaymentAcceptedEvent : Event, INotification
    {
        public PaymentAcceptedEvent(Domain.Payments.Payment payment)
            : base()
        {
            AggregateId = payment.Id;
            When = DateTime.UtcNow;
            Data = payment;
        }
    }
}
