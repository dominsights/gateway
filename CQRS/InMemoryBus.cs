using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public class InMemoryBus : IBus
    {
        public IEventStore EventStore { get; private set; }

        private static readonly IDictionary<Type, Type> RegisteredSagas = new Dictionary<Type, Type>();
        private static readonly IList<Type> RegisteredHandlers = new List<Type>();

        public async Task RaiseEventAsync<T>(T theEvent) where T : Event
        {
            if(EventStore != null)
            {
                await EventStore.SaveAsync(theEvent);
            }

            SendInternal(theEvent);
        }

        public void RegisterHandler<T>()
        {
            RegisteredHandlers.Add(typeof(T));
        }

        public void RegisterSaga<T>() where T : Saga
        {
            var sagaType = typeof(T);
            if(sagaType.GetInterfaces().Count(i => i.Name.StartsWith(typeof(IStartWithMessage<>).Name)) != 1)
            {
                throw new InvalidOperationException("The specified saga must implement the IStartWithMessage<T> interface.");
            }

            var messageType = sagaType.GetInterfaces().First(i => i.Name.StartsWith(typeof(IStartWithMessage<>).Name)).GenericTypeArguments.First();
            RegisteredSagas.Add(messageType, sagaType);
        }

        public void Send<T>(T message) where T : Message
        {
            SendInternal(message);
        }

        private void SendInternal<T>(T message) where T : Message
        {
            LaunchSagasThatStartWithMessage(message);
            DeliverMessageToRunningSagas(message);
            DeliverMessageToRegisteredHandlers(message);
        }

        private void DeliverMessageToRegisteredHandlers<T>(T message) where T : Message
        {
            var messageType = message.GetType();
            var openInterface = typeof(IHandleMessage<>);
            var closedInterface = openInterface.MakeGenericType(messageType);
            var handlersToNotify = from h in RegisteredHandlers
                                   where closedInterface.IsAssignableFrom(h)
                                   select h;

            foreach (var s in handlersToNotify)
            {
                dynamic sagaInstance = Activator.CreateInstance(s, this, EventStore);
                sagaInstance.Handle(message);
            }
        }

        private void DeliverMessageToRunningSagas<T>(T message) where T : Message
        {
            var messageType = message.GetType();
            var openInterface = typeof(IHandleMessage<>);
            var closedInterface = openInterface.MakeGenericType(messageType);
            var sagasToNotify = from s in RegisteredSagas.Values
                                where closedInterface.IsAssignableFrom(s)
                                select s;

            foreach (var s in sagasToNotify)
            {
                dynamic sagaInstance = Activator.CreateInstance(s, this, EventStore);
                sagaInstance.Handle(message);
            }
        }

        private void LaunchSagasThatStartWithMessage<T>(T message) where T : Message
        {

            var messageType = message.GetType();
            var openInterface = typeof(IStartWithMessage<>);
            var closedInterface = openInterface.MakeGenericType(messageType);
            var sagasToLaunch = from s in RegisteredSagas.Values
                                where closedInterface.IsAssignableFrom(s)
                                select s;

            foreach (var s in sagasToLaunch)
            {
                dynamic sagaInstance = Activator.CreateInstance(s, this, EventStore);
                sagaInstance.Handle(message);
            }
        }

        public InMemoryBus(IEventStore eventStore)
        {
            EventStore = eventStore;
        }
    }
}
