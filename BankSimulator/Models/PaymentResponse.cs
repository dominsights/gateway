using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSimulator.Models
{
    public class PaymentResponse
    {
        public Guid Id { get; set; }
        public Status Status { get; set; }
    }
}
