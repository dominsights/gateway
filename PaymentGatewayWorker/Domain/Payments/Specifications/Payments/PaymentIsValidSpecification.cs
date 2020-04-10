using DomainValidationCore.Interfaces.Specification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentGatewayWorker.Domain.Payments.Specifications.Payments
{
    class PaymentIsValidSpecification : ISpecification<Payment>
    {
        public bool IsSatisfiedBy(Payment payment)
        {
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(payment, new ValidationContext(payment), validationResults);
        }
    }
}
