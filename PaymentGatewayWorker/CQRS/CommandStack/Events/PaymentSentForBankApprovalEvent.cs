using MediatR;
using System;

namespace PaymentGatewayWorker.CQRS.CommandStack.Events
{
    public class PaymentSentForBankApprovalEvent : Event, INotification
    {
        public PaymentSentForBankApprovalEvent(Guid aggregateId, Guid bankResponseId)
        {
            AggregateId = aggregateId;
            BankResponseId = bankResponseId;
        }

        public Guid BankResponseId { get; }
    }
}
