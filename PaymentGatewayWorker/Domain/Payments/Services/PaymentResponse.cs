using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.Domain.Payments.Services
{
    public class PaymentHubResponse
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
    }
}
