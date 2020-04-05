using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

/* Assumptions:
 * - Currency code is 3 chars length, example: (EUR, USD, BRL);
 * - CVV size is always 3 chars long;
 */

namespace PaymentGateway.Payments.Services
{
    public class PaymentDto
    {
        public Guid Id { get; internal set; }
        [Required]
        public Guid UserId { get; internal set; }
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
    }
}
