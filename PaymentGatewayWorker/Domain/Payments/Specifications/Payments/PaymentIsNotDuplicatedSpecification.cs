using System;
using System.Collections.Generic;
using System.Text;
using DomainValidationCore.Interfaces.Specification;
using PaymentGatewayWorker.Domain.Payments.Data;
using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using PaymentGatewayWorker.Domain.Payments.Data.Repository;

namespace PaymentGatewayWorker.Domain.Specifications.Payments
{
    class PaymentIsNotDuplicatedSpecification : ISpecification<Domain.Payments.Payment>
    {
        private PaymentRepository _paymentRepository;

        public PaymentIsNotDuplicatedSpecification(PaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public bool IsSatisfiedBy(Domain.Payments.Payment entity)
        {
            // I'm assuming these field are enough to check if the payment is duplicated.
            var paymentFilter = new PaymentFilter()
            {
                Amount = entity.Amount,
                CardNumber = entity.CardNumber,
                CurrencyCode = entity.CurrencyCode,
                CVV = entity.CVV
            };

            // TODO: Extend ISpecification to support async
            return _paymentRepository.GetByFilterAsync(paymentFilter).Result == null;
        }
    }
}
