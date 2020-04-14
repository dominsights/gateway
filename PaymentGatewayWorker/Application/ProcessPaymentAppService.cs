using AutoMapper;
using CQRS;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGatewayWorker.CQRS;
using PaymentGatewayWorker.Domain;
using PaymentGatewayWorker.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker
{
    class ProcessPaymentAppService
    {
        private ILogger<ProcessPaymentAppService> _logger;
        private IMapper _mapper;
        private PaymentService _paymentService;
        private IMediator _mediator;

        internal void ProcessPayments(PaymentDto paymentDto)
        {
            var payment = _mapper.Map<Domain.Payments.Payment>(paymentDto);
            var paymentResult = _paymentService.ValidateToCreate(payment);

            if (paymentResult.ValidationResult.IsValid)
            {
                var command = new AddNewPaymentCommand(
                    payment.UserId,
                    payment.Id,
                    payment.CardNumber,
                    payment.ExpiryMonth,
                    payment.ExpiryYear,
                    payment.Amount,
                    payment.CurrencyCode,
                    payment.CVV);

                _mediator.Send(command);
            }
            else
            {
                string message = "Payment details are invalid.";
                _logger.LogError(message, paymentResult);
                throw new InvalidOperationException(message);
            }
        }

        public ProcessPaymentAppService(ILogger<ProcessPaymentAppService> logger, IMapper mapper, PaymentService paymentService, IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _paymentService = paymentService;
            _mediator = mediator;
        }
    }
}
