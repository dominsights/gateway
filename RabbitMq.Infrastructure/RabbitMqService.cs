using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq.Infrastructure
{
    public class RabbitMqService : IRabbitMqPublisher, IRabbitMqConsumer, IDisposable
    {
        private const string QUEUE_NAME = "payment_queue";
        private ILogger _logger;
        private ConcurrentDictionary<ulong, string> _outstandingConfirms;
        private RabbitMqConfig _rabbitMQConfig;
        private IConnection _connection;
        private IModel _channel;

        public virtual Task SendPaymentAsync(string paymentSerialized)
        {
            Task task = new Task(() =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitMQConfig.HostName,
                    UserName = _rabbitMQConfig.UserName,
                    Password = _rabbitMQConfig.Password
                };

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ConfirmSelect();
                        channel.BasicAcks += Channel_BasicAcks;
                        channel.BasicNacks += Channel_BasicNacks;

                        channel.QueueDeclare(queue: QUEUE_NAME, durable: true, exclusive: false,
                                              autoDelete: false, arguments: null);

                        var body = Encoding.UTF8.GetBytes(paymentSerialized);

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        _outstandingConfirms.TryAdd(channel.NextPublishSeqNo, paymentSerialized);
                        channel.BasicPublish(exchange: string.Empty, routingKey: QUEUE_NAME,
                                              basicProperties: properties, body: body);

                        channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
                    }
                }
            });

            task.Start();

            return task;
        }

        public Task StartListeningForPaymentRequests()
        {
            var task = new Task(() =>
            {

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
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    var message = Encoding.UTF8.GetString(body);

                    // Do Work
                    Console.WriteLine(message);

                    _channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: true);
                };

                _channel.BasicConsume(queue: QUEUE_NAME, autoAck: false, consumer: consumer);
            });

            task.Start();

            return task;
        }

        private void ClearOutstandingConfirms(ulong sequenceNumber, bool multiple)
        {
            if (multiple)
            {
                var confirmed = _outstandingConfirms.Where(k => k.Key <= sequenceNumber);

                foreach (var entry in confirmed)
                {
                    _outstandingConfirms.TryRemove(entry.Key, out _);
                }
            }
            else
            {
                _outstandingConfirms.TryRemove(sequenceNumber, out _);
            }
        }

        private void Channel_BasicAcks(object sender, BasicAckEventArgs e)
        {
            ClearOutstandingConfirms(e.DeliveryTag, e.Multiple);
        }

        private void Channel_BasicNacks(object sender, BasicNackEventArgs e)
        {
            _outstandingConfirms.TryGetValue(e.DeliveryTag, out string body);
            _logger.LogError($"Message with body {body} has been nack-ed. Sequence number: {e.DeliveryTag}, multiple: {e.Multiple}");

            ClearOutstandingConfirms(e.DeliveryTag, e.Multiple);
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

        public RabbitMqService(ILogger<RabbitMqService> logger, IOptions<RabbitMqConfig> rabbitMQConfig)
        {
            _logger = logger;
            _outstandingConfirms = new ConcurrentDictionary<ulong, string>();
            _rabbitMQConfig = rabbitMQConfig.Value;
        }

        // Necessary for mocking
        protected RabbitMqService()
        {

        }
    }
}
