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

        public bool IsSatisfiedBy(Payment entity)
        {
            // check wether the events for the payment has a status of approved or denied

            return _eventRepository.IsNotApprovedOrDenied(entity.Id).Result;
        }
    }
}
