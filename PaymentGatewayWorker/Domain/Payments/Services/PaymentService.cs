using PaymentGatewayWorker.Domain.Payments.Data;
using PaymentGatewayWorker.Domain.Payments.Data.Repository;
using PaymentGatewayWorker.Domain.Payments.Services;
using PaymentGatewayWorker.Domain.Payments.Validations.Payments;
using PaymentGatewayWorker.Domain.Validations.Payments;
using PaymentGatewayWorker.EventSourcing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.Domain.Services
{
    class PaymentService
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly EventRepository _eventRepository;

        public Payments.Payment ValidateToCreate(Payments.Payment payment)
        {
            if(!payment.IsValid())
            {
                return payment;
            }

            payment.ValidationResult = new PaymentIsIdempotentValidation(_paymentRepository).Validate(payment);

            return payment;
        }

        public PaymentService(PaymentRepository paymentRepository, EventRepository eventRepository)
        {
            _paymentRepository = paymentRepository;
            _eventRepository = eventRepository;
        }

        internal async Task<Payments.Payment> ValidateToUpdateStatusAsync(PaymentHubResponse response)
        {
            var payment = await _paymentRepository.GetByBankResponseIdAsync(response.Id);

            if (!payment.IsValid())
            {
                return payment;
            }

            payment.ValidationResult = new PaymentIsAlreadyValidatedByBankValidation(_eventRepository).Validate(payment);

            return payment;
        }
    }
}
