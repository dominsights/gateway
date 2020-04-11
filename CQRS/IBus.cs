using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public interface IBus
    {
        void Send<T>(T message) where T : Message;
        Task RaiseEventAsync<T>(T theEvent) where T : Event;
        void RegisterSaga<T>() where T : Saga;
        void RegisterHandler<T>();
    }
}
