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

namespace PaymentGateway.Payments.Services
{
    public class RabbitMqService : IMessagingService
    {
        private ILogger _logger;
        private ConcurrentDictionary<ulong, string> _outstandingConfirms;
        private RabbitMqConfig _rabbitMQConfig;

        public RabbitMqService(ILogger<RabbitMqService> logger, IOptions<RabbitMqConfig> rabbitMQConfig)
        {
            _logger = logger;
            _outstandingConfirms = new ConcurrentDictionary<ulong, string>();
            _rabbitMQConfig = rabbitMQConfig.Value;
        }

        public Task SendPaymentAsync(string paymentSerialized)
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

                        channel.QueueDeclare(queue: "payment_queue", durable: true, exclusive: false,
                                              autoDelete: false, arguments: null);

                        var body = Encoding.UTF8.GetBytes(paymentSerialized);

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        _outstandingConfirms.TryAdd(channel.NextPublishSeqNo, paymentSerialized);
                        channel.BasicPublish(exchange: string.Empty, routingKey: "payment_queue",
                                              basicProperties: properties, body: body);

                        channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
                    }
                }
            });

            task.Start();

            return task;
        }

        private void CleanOutstandingConfirms(ulong sequenceNumber, bool multiple)
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
            CleanOutstandingConfirms(e.DeliveryTag, e.Multiple);
        }

        private void Channel_BasicNacks(object sender, BasicNackEventArgs e)
        {
            _outstandingConfirms.TryGetValue(e.DeliveryTag, out string body);
            _logger.LogError($"Message with body {body} has been nack-ed. Sequence number: {e.DeliveryTag}, multiple: {e.Multiple}");

            CleanOutstandingConfirms(e.DeliveryTag, e.Multiple);
        }
    }
}
