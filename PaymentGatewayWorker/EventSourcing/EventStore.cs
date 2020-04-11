using CQRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.EventSourcing
{
    class EventStore : IEventStore
    {
        private EventRepository _eventRepository;

        public async Task SaveAsync<T>(T @event) where T : Event
        {
            var loggedEvent = new LoggedEvent()
            {
                Action = @event.Name,
                AggregateId = @event.AggregateId,
                Data = JsonSerializer.Serialize(@event)
            };

            await _eventRepository.SaveAsync(loggedEvent);
        }

        public EventStore(EventRepository eventRepository) 
        {
            _eventRepository = eventRepository;
        }
    }
}
