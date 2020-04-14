using MediatR;
using System;

namespace PaymentGatewayWorker.CQRS.CommandStack.Events
{
    public class PaymentCreatedEvent : Event, INotification
    {
        public PaymentCreatedEvent(Guid id, Domain.Payments.Payment data)
            : base()
        {
            AggregateId = id;
            When = DateTime.UtcNow;
            Data = data;
        }

        public DateTime When { get; set; }
        public Domain.Payments.Payment Data { get; set; }
    }
}
