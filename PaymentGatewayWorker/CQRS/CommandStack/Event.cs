using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.CQRS.CommandStack
{
    public class Event : Message
    {
        public DateTime When { get; private set; }

        public Event()
            : base()
        {
            When = DateTime.UtcNow;
            Name = GetType().Name;
        }
    }
}
