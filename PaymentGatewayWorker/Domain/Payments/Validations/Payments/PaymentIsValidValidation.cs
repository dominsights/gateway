using DomainValidationCore.Validation;
using PaymentGatewayWorker.Domain.Payments.Specifications.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.Domain.Payments.Validations.Payments
{
    class PaymentIsValidValidation : Validator<Payment>
    {
        public PaymentIsValidValidation()
        {
            var paymentIsValid = new PaymentIsValidSpecification();
            base.Add("paymentIsValid", new Rule<Payment>(paymentIsValid, "Payment has some invalid data."));
        }
    }
}
