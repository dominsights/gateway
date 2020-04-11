﻿using PaymentGatewayWorker.Domain.Payments.Data;
using PaymentGatewayWorker.Domain.Validations.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.Domain.Services
{
    class PaymentService
    {
        private readonly PaymentRepository _paymentRepository;

        public Payment ValidateToCreate(Payment payment)
        {
            if(!payment.IsValid())
            {
                return payment;
            }

            payment.ValidationResult = new PaymentIsIdempotentValidation(_paymentRepository).Validate(payment);

            return payment;
        }

        public PaymentService(PaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
    }
}
