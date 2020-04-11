using DomainValidationCore.Validation;
using PaymentGatewayWorker.Domain.Payments.Data;
using PaymentGatewayWorker.Domain.Specifications.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.Domain.Validations.Payments
{
    class PaymentIsIdempotentValidation : Validator<Payment>
    {
        public PaymentIsIdempotentValidation(PaymentRepository paymentRepository)
        {
            var duplicatedId = new PaymentCannotHaveDuplicatedIdSpecification(paymentRepository);
            var duplicatedPayment = new PaymentIsNotDuplicatedSpecification(paymentRepository);

            base.Add("duplicatedId", new Rule<Payment>(duplicatedId, "A payment with the same id already exists."));
            base.Add("duplicatedPayment", new Rule<Payment>(duplicatedPayment, "A payment has already been made with the same payment details already exists."));
        }
    }
}
