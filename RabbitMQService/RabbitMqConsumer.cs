using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQService
{
    public delegate void DoWork(string message);
    public class RabbitMqConsumer
    {
        private IConnection _connection;
        private IModel _channel;
        // private const string QUEUE_NAME = "payment_queue";
        private RabbitMqConfig _rabbitMQConfig;

        public event DoWork DoWork;

        public void StartListeningForPaymentRequests(string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQConfig.HostName,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false,
                                    autoDelete: false, arguments: null);

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body;
                var message = Encoding.UTF8.GetString(body);

                DoWork(message);

                _channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: true);
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
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

        public RabbitMqConsumer(ILogger<RabbitMqConsumer> logger, IOptions<RabbitMqConfig> rabbitMQConfig) : this()
        {
            _rabbitMQConfig = rabbitMQConfig.Value;
        }

        // Necessary for mocking
        protected RabbitMqConsumer() : base()
        {

        }
    }
}
