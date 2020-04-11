using System.Threading.Tasks;

namespace CQRS
{
    public interface IEventStore
    {
        Task SaveAsync<T>(T theEvent) where T : Event;
    }
}