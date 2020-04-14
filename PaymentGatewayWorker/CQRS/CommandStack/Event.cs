using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.CQRS.CommandStack
{
    public class Event : Message
    {
        public DateTime TimeStamp { get; private set; }
        public DateTime When { get; set; }
        public Domain.Payments.Payment Data { get; set; }

        public Event()
            : base()
        {
            TimeStamp = DateTime.UtcNow;
            Name = GetType().Name;
        }
    }
}
