using CQRS;
using PaymentGatewayWorker.CQRS.CommandStack.Sagas;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.CQRS
{
    class BusConfig
    {
        public static void Initialize(IBus bus)
        {
            bus.RegisterSaga<AddPaymentSaga>();
        }
    }
}
