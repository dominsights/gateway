using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using RabbitMQService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.CQRS.CommandStack.Handlers
{
    class PaymentAcceptedEventHandler : INotificationHandler<PaymentAcceptedEvent>
    {
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        private readonly ILogger<PaymentAcceptedEventHandler> _logger;

        public async Task Handle(PaymentAcceptedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var response = new PaymentResponse(notification.AggregateId, "APPROVED");
                await _rabbitMqPublisher.SendMessageAsync(JsonSerializer.Serialize(response), "response_queue");
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error occured while trying to payment response to rabbitmq.");
            }
        }

        public PaymentAcceptedEventHandler(RabbitMqPublisher rabbitMqPublisher, ILogger<PaymentAcceptedEventHandler> logger)
        {
            _rabbitMqPublisher = rabbitMqPublisher;
            _logger = logger;
        }
    }
}
