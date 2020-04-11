using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PaymentGatewayWorker.Domain.Payments.Interfaces
{
    interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment> GetByIdAsync(Guid id);
        Task<Payment> GetByFilterAsync(PaymentFilter paymentFilter);
    }
}
