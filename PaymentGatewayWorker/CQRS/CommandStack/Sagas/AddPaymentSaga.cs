using CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.CQRS.CommandStack.Sagas
{
    public class AddPaymentSaga : Saga,
        IStartWithMessage<AddNewPaymentCommand>
    {
        public void Handle(AddNewPaymentCommand message)
        {
            throw new NotImplementedException();
        }

        public AddPaymentSaga(IBus bus, IEventStore eventStore)
            : base(bus, eventStore)
        {

        }

        //public AddPaymentSaga(IBus bus, IEventStore eventStore, IRepository repository)
        //    : base(bus, eventStore)
        //{

        //}
    }
}
