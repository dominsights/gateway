using PaymentGatewayWorker.CQRS.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Text;
using CQRS;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.CQRS.CommandStack.Handlers
{
    public class AddNewPaymentErrorHandler
    {
        public Task HandleAsync(AddNewPaymentErrorEvent message)
        {
            // Do something when error
            throw new NotImplementedException();
        }
    }
}
