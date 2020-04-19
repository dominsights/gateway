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

namespace RabbitMQService
{
    public class RabbitMqPublisher
    {
        private ILogger _logger;
        private ConcurrentDictionary<ulong, string> _outstandingConfirms;
        private RabbitMqConfig _rabbitMQConfig;
        private IConnection _connection;
        private IModel _channel;

        public virtual Task SendMessageAsync(string message, string queueName)
        {
            Task task = new Task(() =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitMQConfig.HostName,
                    UserName = _rabbitMQConfig.UserName,
                    Password = _rabbitMQConfig.Password
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ConfirmSelect();
                _channel.BasicAcks += Channel_BasicAcks;
                _channel.BasicNacks += Channel_BasicNacks;

                _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false,
                                                  autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;

                _outstandingConfirms.TryAdd(_channel.NextPublishSeqNo, message);
                _channel.BasicPublish(exchange: string.Empty, routingKey: queueName,
                                              basicProperties: properties, body: body);

                _channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
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

        public RabbitMqPublisher(ILogger<RabbitMqPublisher> logger, IOptions<RabbitMqConfig> rabbitMQConfig) : this()
        {
            _logger = logger;
            _outstandingConfirms = new ConcurrentDictionary<ulong, string>();
            _rabbitMQConfig = rabbitMQConfig.Value;
        }

        // Necessary for mocking
        protected RabbitMqPublisher()
        {

        }
    }
}
