using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Hubs;
using RabbitMQService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway
{
    public class PaymentBackgroundService : BackgroundService
    {
        private readonly RabbitMqConsumer _rabbitMqConsumer;
        private readonly IHubContext<PaymentResponseHub> _hubContext;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _rabbitMqConsumer.MessageReceived += _rabbitMqConsumer_MessageReceived;
            await _rabbitMqConsumer.StartListeningForPaymentRequestsAsync("response_queue");
        }

        private void _rabbitMqConsumer_MessageReceived(string message)
        {
            var response = JsonSerializer.Deserialize<PaymentResponse>(message);
            // TODO: Use proper method to send message to the correct client
            _hubContext.Clients.All.SendAsync("PaymentResponse", response);
        }

        public PaymentBackgroundService(RabbitMqConsumer rabbitMqConsumer, IHubContext<PaymentResponseHub> hubContext)
        {
            _rabbitMqConsumer = rabbitMqConsumer;
            _hubContext = hubContext;
        }
    }
}
