using CQRS;
using Microsoft.Extensions.Logging;
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
        private ILogger<EventStore> _logger;

        public async Task SaveAsync<T>(T @event) where T : Event
        {
            var loggedEvent = new LoggedEvent()
            {
                Action = @event.Name,
                AggregateId = @event.AggregateId,
                Data = JsonSerializer.Serialize(@event)
            };

            try
            {
                await _eventRepository.SaveAsync(loggedEvent);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error while trying to save logged event.");
            }
        }

        public EventStore(EventRepository eventRepository, ILogger<EventStore> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }
    }
}
