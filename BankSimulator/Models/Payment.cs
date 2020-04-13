using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSimulator.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; } // shopper identifier to send the payment
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string CVV { get; set; }
    }
}
