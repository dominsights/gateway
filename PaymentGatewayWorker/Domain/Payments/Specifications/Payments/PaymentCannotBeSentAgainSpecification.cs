using System;
using System.Collections.Generic;
using System.Text;
using CQRS;
using DomainValidationCore.Interfaces.Specification;
using PaymentGatewayWorker.EventSourcing;

namespace PaymentGatewayWorker.Domain.Payments.Specifications.Payments
{
    class PaymentCannotBeSentAgainSpecification : ISpecification<Payment>
    {
        private EventRepository _eventRepository;

        public PaymentCannotBeSentAgainSpecification(EventRepository eventRepository)
        {
            this._eventRepository = eventRepository;
        }

        public bool IsSatisfiedBy(Payment payment)
        {
            return _eventRepository.IsNotApprovedOrDenied(payment.Id).Result;
        }
    }
}
