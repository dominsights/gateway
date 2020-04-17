using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Payments.Models
{
    public class PaymentDetailsDto
    {
        public Guid Id { get; set; }
        // mask the card number as *********0458 or save it already with this format (the whole class)
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Status { get; internal set; }
    }
}
