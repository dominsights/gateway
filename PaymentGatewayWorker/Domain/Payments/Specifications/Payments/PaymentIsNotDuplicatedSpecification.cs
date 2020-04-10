using System;
using System.Collections.Generic;
using System.Text;
using DomainValidationCore.Interfaces.Specification;
using PaymentGatewayWorker.Domain.Interfaces;

namespace PaymentGatewayWorker.Domain.Specifications.Payments
{
    class PaymentIsNotDuplicatedSpecification : ISpecification<Payment>
    {
        private IPaymentRepository paymentRepository;

        public PaymentIsNotDuplicatedSpecification(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public bool IsSatisfiedBy(Payment entity)
        {
            // query to check if there is a payment with the same payment details
            throw new NotImplementedException();
        }
    }
}
