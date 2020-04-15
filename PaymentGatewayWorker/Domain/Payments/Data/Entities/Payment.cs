using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PaymentGatewayWorker.Domain.Payments.Data.Entities
{
    [Table("Payment")]
    class Payment
    {
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public int ExpiryMonth { get; set; }
        [Required]
        public int ExpiryYear { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string CurrencyCode { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string CVV { get; set; }
        public BankResponse BankResponse { get; internal set; }
    }
}
