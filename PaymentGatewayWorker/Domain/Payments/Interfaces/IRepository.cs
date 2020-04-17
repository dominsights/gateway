using PaymentGatewayWorker.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public interface IRepository
    {
        Task<CommandResponse> CreateFromRequestAsync<T>(T item) where T : class;
    }
}
