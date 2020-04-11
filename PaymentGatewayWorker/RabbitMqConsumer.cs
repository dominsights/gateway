using CQRS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentGatewayWorker.CQRS;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentGatewayWorker
{
    class RabbitMqConsumer
    {
        private IConnection _connection;
        private IModel _channel;
        private const string QUEUE_NAME = "payment_queue";
        private ILogger<RabbitMqConsumer> _logger;
        private RabbitMqConfig _rabbitMQConfig;
        private ProcessPaymentAppService _processPaymentAppService;
        private IBus _bus;

        public void StartListeningForPaymentRequests()
        {
            BusConfig.Initialize(_bus);

            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QUEUE_NAME, durable: true, exclusive: false,
                                    autoDelete: false, arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body);

                await DoWorkAsync(message);

                _channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: true);
            };

            _channel.BasicConsume(queue: QUEUE_NAME, autoAck: false, consumer: consumer);
        }

        private async Task DoWorkAsync(string message)
        {
            await Task.Run(() =>
            {
                var paymentDto = JsonSerializer.Deserialize<PaymentDto>(message);
                _processPaymentAppService.ProcessPayments(paymentDto);
            });
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _channel?.Dispose();
                    _connection?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public RabbitMqConsumer(ILogger<RabbitMqConsumer> logger, IOptions<RabbitMqConfig> rabbitMQConfig, ProcessPaymentAppService processPaymentAppService, IBus bus) : this()
        {
            _logger = logger;
            _rabbitMQConfig = rabbitMQConfig.Value;
            _processPaymentAppService = processPaymentAppService;
            _bus = bus;
        }

        // Necessary for mocking
        protected RabbitMqConsumer() : base()
        {

        }
    }
}
