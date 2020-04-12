using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS
{
    public class Event : Message
    {
        public DateTime TimeStamp { get; private set; }

        public Event()
            :base()
        {
            TimeStamp = DateTime.UtcNow;
            Name = this.GetType().Name;
        }
    }
}
