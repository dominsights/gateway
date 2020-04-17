using PaymentGatewayWorker.Domain.Payments.Validations.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PaymentGatewayWorker.Domain.Payments
{
    public class Payment
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

        public DomainValidationCore.Validation.ValidationResult ValidationResult { get; internal set; }

        internal bool IsValid()
        {
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this, new ValidationContext(this), validationResults);

            ValidationResult = new DomainValidationCore.Validation.ValidationResult();

            if (!isValid)
            {
                foreach (var result in validationResults)
                {
                    ValidationResult.Add(new DomainValidationCore.Validation.ValidationError(result.MemberNames.First(), result.ErrorMessage));
                }
            }

            return ValidationResult.IsValid;
        }

        public static class Factory
        {
            public static Payment NewPayment(Guid id, Guid userId, string cardNumber, int expiryMonth, int expiryYear, decimal amount, string currencyCode, string cvv)
            {
                var payment = new Payment()
                {
                    Id = id,
                    UserId = userId,
                    Amount = amount,
                    CardNumber = cardNumber,
                    CurrencyCode = currencyCode,
                    CVV = cvv,
                    ExpiryMonth = expiryMonth,
                    ExpiryYear = expiryYear
                };

                return payment;
            }
        }
    }
}
