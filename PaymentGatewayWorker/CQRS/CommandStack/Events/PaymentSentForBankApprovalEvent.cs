using CQRS;
using PaymentGatewayWorker.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.CQRS.CommandStack.Events
{
    public class PaymentSentForBankApprovalEvent : Event
    {
        public PaymentSentForBankApprovalEvent(Guid id, Payment data)
            : base()
        {
            AggregateId = id;
            When = DateTime.UtcNow;
            Data = data;
        }

        public DateTime When { get; set; }
        public Payment Data { get; set; }
    }
}
