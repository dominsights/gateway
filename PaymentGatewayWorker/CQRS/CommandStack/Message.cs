using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.CQRS.CommandStack
{
    public class Message
    {
        public Guid AggregateId { get; protected set; }
        public string Name { get; protected set; }
    }
}
