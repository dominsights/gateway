using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq.Infrastructure
{
    public interface IRabbitMqConsumer
    {
        Task StartListeningForPaymentRequests();
    }
}
