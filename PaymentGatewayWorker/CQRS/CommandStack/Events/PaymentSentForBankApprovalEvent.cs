using MediatR;
using System;

namespace PaymentGatewayWorker.CQRS.CommandStack.Events
{
    public class PaymentSentForBankApprovalEvent : Event, IRequest
    {
        public PaymentSentForBankApprovalEvent(Guid id, Domain.Payments.Payment data)
            : base()
        {
            AggregateId = id;
            When = DateTime.UtcNow;
            Data = data;
        }


    }
}
