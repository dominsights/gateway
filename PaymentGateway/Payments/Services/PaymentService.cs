using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Payments.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<Guid> ProcessPaymentAsync(PaymentDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
