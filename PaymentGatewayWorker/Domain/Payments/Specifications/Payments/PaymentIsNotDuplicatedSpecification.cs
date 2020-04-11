using System;
using System.Collections.Generic;
using System.Text;
using DomainValidationCore.Interfaces.Specification;
using PaymentGatewayWorker.Domain.Payments.Data;

namespace PaymentGatewayWorker.Domain.Specifications.Payments
{
    class PaymentIsNotDuplicatedSpecification : ISpecification<Payment>
    {
        private PaymentRepository _paymentRepository;

        public PaymentIsNotDuplicatedSpecification(PaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public bool IsSatisfiedBy(Payment entity)
        {
            // query to check if there is a payment with the same payment details
            throw new NotImplementedException();
        }
    }
}
