using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DomainValidationCore.Interfaces.Specification;
using PaymentGatewayWorker.Domain.Payments.Data;
using PaymentGatewayWorker.Domain.Payments.Data.Repository;
using PaymentGatewayWorker.Domain.Payments.Interfaces;

namespace PaymentGatewayWorker.Domain.Specifications.Payments
{
    class PaymentCannotHaveDuplicatedIdSpecification : ISpecification<Domain.Payments.Payment>
    {
        private IPaymentRepository _paymentRepository;

        public PaymentCannotHaveDuplicatedIdSpecification(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public bool IsSatisfiedBy(Domain.Payments.Payment entity)
        {
            // TODO: Extend ISpecification to support async
            return _paymentRepository.GetByIdAsync(entity.Id).Result == null;
        }
    }
}
