using PaymentGatewayWorker.CQRS.CommandStack;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.EventSourcing
{
    public interface IEventStore
    {
        Task SaveAsync<T>(T theEvent) where T : Event;
    }
}