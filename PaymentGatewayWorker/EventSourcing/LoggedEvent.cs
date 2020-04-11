using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.EventSourcing
{
    public class LoggedEvent
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public Guid AggregateId { get; set; }
        public string Data { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
