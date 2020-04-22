using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentGatewayWorker.CQRS.CommandStack.Commands;
using PaymentGatewayWorker.Domain.Payments.Services;
using RabbitMQService;

namespace PaymentGatewayWorker
{
    class Worker : BackgroundService
    {
        private const string PAYMENT_HUB = "paymentHub";
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMqConsumer _rabbitMqConsumer;
        private readonly IMediator _mediator;
        private readonly BankAPIConfig _signalRConfig;
        private readonly ProcessPaymentAppService _processPaymentAppService;
        HubConnection _connection;

        public Worker(ILogger<Worker> logger, RabbitMqConsumer rabbitMqConsumer, IMediator mediator, IOptions<BankAPIConfig> signalRConfig, ProcessPaymentAppService processPaymentAppService)
        {
            _logger = logger;
            _rabbitMqConsumer = rabbitMqConsumer;
            _mediator = mediator;
            _signalRConfig = signalRConfig.Value;
            _processPaymentAppService = processPaymentAppService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start listening for SignalR Hub
            await StartListeningToSignalRAsync(stoppingToken);

            // Start listening for new payment requests
            _rabbitMqConsumer.MessageReceived += ProcessPayment;
            await _rabbitMqConsumer.StartListeningForPaymentRequestsAsync("payment_queue");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async void ProcessPayment(string message)
        {
            await Task.Run(() =>
            {
                try
                {
                    var paymentDto = JsonSerializer.Deserialize<PaymentDto>(message);
                    _processPaymentAppService.ProcessPayments(paymentDto);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while processing message.", message);
                }
            });
        }

        private async Task StartListeningToSignalRAsync(CancellationToken stoppingToken)
        {
            _connection = new HubConnectionBuilder()
                         .WithAutomaticReconnect()
                         .WithUrl(_signalRConfig.ServerUrl + PAYMENT_HUB)
                         .Build();

            _connection.On<PaymentHubResponse>("PaymentResponse", (response) =>
            {
                _mediator.Send(new UpdatePaymentStatusWithBankResponseCommand(response));
            });

            await ConnectWithRetryAsync(stoppingToken);
        }

        private async Task ConnectWithRetryAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                try
                {
                    await _connection.StartAsync();
                    Debug.Assert(_connection.State == HubConnectionState.Connected);
                    return;
                }
                catch when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }
                catch
                {
                    Debug.Assert(_connection.State == HubConnectionState.Disconnected);
                    _logger.LogError($"Trying to connect to {_signalRConfig.ServerUrl + PAYMENT_HUB} failed. Check if the url is correct and update the appSettings value to the correct one.");
                    await Task.Delay(5000);
                }
            }
        }
    }
}
