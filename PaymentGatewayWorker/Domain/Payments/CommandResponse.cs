using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.Domain.Payments
{
    public class CommandResponse
    {
        public static CommandResponse Ok = new CommandResponse { Success = true };
        public static CommandResponse Fail = new CommandResponse { Success = false };

        public bool Success { get; private set; }
        public Guid AggregateId { get; private set; }
        public Guid RequestId { get; private set; }
        public string Description { get; set; }

        public CommandResponse(bool success = false, Guid aggregateId = new Guid())
        {
            Success = success;
            AggregateId = aggregateId;
        }
    }
}
