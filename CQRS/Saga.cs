using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS
{
    public abstract class Saga
    {
        private IBus bus;
        private IEventStore eventStore;

        protected Saga(IBus bus, IEventStore eventStore)
        {
            if(bus == null)
            {
                throw new ArgumentNullException("bus");
            }
            this.bus = bus;
            this.eventStore = eventStore;
        }

        public IBus Bus { get; private set; }
        public IEventStore EventStore { get; private set; }
    }
}
