using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Payments.Application
{
    public interface IPaymentAppService
    {
        Task<Guid> ProcessPaymentAsync(PaymentDto dto);
    }
}
