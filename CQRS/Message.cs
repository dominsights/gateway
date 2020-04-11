using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS
{
    public class Message
    {
        public Guid AggregateId { get; protected set; }
        public string Name { get; protected set; }
    }
}
