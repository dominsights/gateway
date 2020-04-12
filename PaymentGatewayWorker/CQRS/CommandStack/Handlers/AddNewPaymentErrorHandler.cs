using PaymentGatewayWorker.CQRS.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Text;
using CQRS;

namespace PaymentGatewayWorker.CQRS.CommandStack.Handlers
{
    public class AddNewPaymentErrorHandler : IHandleMessage<AddNewPaymentErrorEvent>
    {
        public void Handle(AddNewPaymentErrorEvent message)
        {
            // Do something when error
            throw new NotImplementedException();
        }
    }
}
