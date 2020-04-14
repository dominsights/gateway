using System;
using System.Collections.Generic;
using System.Text;
using DomainValidationCore.Interfaces.Specification;
using PaymentGatewayWorker.EventSourcing;

namespace PaymentGatewayWorker.Domain.Payments.Specifications.Payments
{
    class PaymentCannotBeSentAgainSpecification : ISpecification<Payment>
    {
        private EventRepository eventRepository;

        public PaymentCannotBeSentAgainSpecification(EventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public bool IsSatisfiedBy(Payment entity)
        {
            // check wether the events for the payment has a status of approved or denied
            throw new NotImplementedException();
        }
    }
}
