using AutoMapper;
using Microsoft.Extensions.Logging;
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

        internal Task ProcessPaymentAsync(PaymentDto paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            var paymentResult = _paymentService.ValidateToCreate(payment);

            if(paymentResult.ValidationResult.IsValid)
            {
                // create command
            }

            throw new NotImplementedException();
        }

        public ProcessPaymentAppService(ILogger<ProcessPaymentAppService> logger, IMapper mapper, PaymentService paymentService)
        {
            _logger = logger;
            _mapper = mapper;
            _paymentService = paymentService;
        }
    }
}
