using PaymentGatewayWorker.Domain.Payments.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGatewayWorker.Domain.Payments.Data.Entities
{
    [Table("BankResponse")]
    class BankResponse
    {
        public Guid Id { get; set; }
        [Required]
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}