using CQRS;
using DomainValidationCore.Validation;
using PaymentGatewayWorker.Domain.Payments.Specifications.Payments;
using PaymentGatewayWorker.EventSourcing;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.Domain.Payments.Validations.Payments
{
    class PaymentIsAlreadyValidatedByBankValidation : Validator<Payment>
    {
        public PaymentIsAlreadyValidatedByBankValidation(EventRepository eventRepository)
        {
            var specification = new PaymentCannotBeSentAgainSpecification(eventRepository);

            base.Add("specification", new Rule<Payment>(specification, "The payment is wether already denied or approved and cannot be sent again."));
        }
    }
}
