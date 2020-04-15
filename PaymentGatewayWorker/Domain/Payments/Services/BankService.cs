using AutoMapper;
using CQRS;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using PaymentGatewayWorker.CQRS.CommandStack.Commands;
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
        private BankApiClient _bankApiClient;

        public virtual async Task<Guid> SendPaymentForBankApprovalAsync(Payment payment)
        {
            PaymentIsAlreadyValidatedByBankValidation paymentIsOkToSendToBankValidation = new PaymentIsAlreadyValidatedByBankValidation(_eventRepository);

            if (!payment.IsValid() || !paymentIsOkToSendToBankValidation.Validate(payment).IsValid)
            {
                throw new ArgumentException("Payment is invalid.");
            }

            var request = new PaymentGatewayWorker.Payment
            {
                Amount = (double)payment.Amount,
                CardNumber = payment.CardNumber,
                CurrencyCode = payment.CurrencyCode,
                Cvv = payment.CVV,
                ExpiryMonth = payment.ExpiryMonth,
                ExpiryYear = payment.ExpiryYear,
                SellerId = payment.UserId
            };

            Guid bankProcessId = await _bankApiClient.PaymentAsync(request);
            return bankProcessId;
        }

        public BankService(EventRepository eventRepository)
        {
            var httpClient = new HttpClient();
            var client = new BankApiClient("https://localhost:5001/", httpClient);

            _eventRepository = eventRepository;
            _bankApiClient = client;
        }

        // Necessary for mocking
        protected BankService()
        {

        }
    }
}
