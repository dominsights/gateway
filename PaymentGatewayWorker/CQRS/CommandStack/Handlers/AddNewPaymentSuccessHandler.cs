using CQRS;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.CQRS.CommandStack.Handlers
{
    public class AddNewPaymentSuccessHandler : IHandleMessage<AddNewPaymentSuccessEvent>
    {
        public void Handle(AddNewPaymentSuccessEvent message)
        {
            // Do something when success
            throw new NotImplementedException();
        }
    }
}
