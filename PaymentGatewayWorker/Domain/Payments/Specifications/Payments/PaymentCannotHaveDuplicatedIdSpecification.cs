using System;
using System.Collections.Generic;
using System.Text;
using DomainValidationCore.Interfaces.Specification;
using PaymentGatewayWorker.Domain.Payments.Data;

namespace PaymentGatewayWorker.Domain.Specifications.Payments
{
    class PaymentCannotHaveDuplicatedIdSpecification : ISpecification<Payment>
    {
        private PaymentRepository paymentRepository;

        public PaymentCannotHaveDuplicatedIdSpecification(PaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public bool IsSatisfiedBy(Payment entity)
        {
            // query if there is already a payment with the same id
            throw new NotImplementedException();
        }
    }
}
