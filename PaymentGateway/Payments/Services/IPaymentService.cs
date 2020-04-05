using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Payments.Services
{
    public interface IPaymentService
    {
        Task<Guid> ProcessPaymentAsync(PaymentDto dto);
    }
}
