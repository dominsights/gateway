using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentGatewayWorker.CQRS.CommandStack.Commands;
using PaymentGatewayWorker.Domain.Payments.Services;

namespace PaymentGatewayWorker
{
    class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMqConsumer _rabbitMqConsumer;
        private readonly IMediator _mediator;
        HubConnection _connection;


        public Worker(ILogger<Worker> logger, RabbitMqConsumer rabbitMqConsumer, IMediator mediator)
        {
            _logger = logger;
            _rabbitMqConsumer = rabbitMqConsumer;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start listening for SignalR Hub

            _connection = new HubConnectionBuilder()
             .WithUrl("https://localhost:5001/paymentHub") // TODO: Move to configuration file
             .Build();

            _connection.On<PaymentHubResponse>("PaymentResponse", (response) =>
            {
                _mediator.Send(new UpdatePaymentStatusWithBankResponseCommand(response));
            });

            await _connection.StartAsync();

            // Start listening for new payment requests

            _rabbitMqConsumer.StartListeningForPaymentRequests();
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
