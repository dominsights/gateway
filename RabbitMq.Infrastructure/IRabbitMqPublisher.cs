using System.Threading.Tasks;

namespace RabbitMq.Infrastructure
{
    public interface IRabbitMqPublisher
    {
        Task SendPaymentAsync(string paymentSerialized);
    }
}