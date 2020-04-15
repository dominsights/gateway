using AutoMapper;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using PaymentGatewayWorker.CQRS.CommandStack.Commands;
using PaymentGatewayWorker.Domain.Payments.Facades;
using PaymentGatewayWorker.Domain.Payments.Validations.Payments;
using PaymentGatewayWorker.EventSourcing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.Domain.Payments.Services
{
    class BankService
    {
        private EventRepository _eventRepository;
        private BankApiClientFacade _bankApiClient;

        public virtual async Task<Guid> SendPaymentForBankApprovalAsync(Payment payment)
        {
            PaymentIsAlreadyValidatedByBankValidation paymentIsOkToSendToBankValidation = new PaymentIsAlreadyValidatedByBankValidation(_eventRepository);

            if (!payment.IsValid() || !paymentIsOkToSendToBankValidation.Validate(payment).IsValid)
            {
                throw new ArgumentException("Payment is invalid.");
            }

            Guid bankProcessId = await _bankApiClient.SendPaymentToBankAsync(payment);
            return bankProcessId;
        }

        public BankService(EventRepository eventRepository, BankApiClientFacade bankApiClient)
        {
            _eventRepository = eventRepository;
            _bankApiClient = bankApiClient;
        }

        // Necessary for mocking
        protected BankService()
        {

        }
    }
}
