using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Payments.Application
{
    public class PaymentAppService : IPaymentAppService
    {
        public Task<Guid> ProcessPaymentAsync(PaymentDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
