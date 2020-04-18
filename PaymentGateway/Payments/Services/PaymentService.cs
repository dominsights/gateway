using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDbRepository;
using PaymentGateway.Payments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentGateway.Payments.Services
{
    public class PaymentService
    {
        private RabbitMqPublisher _messagingService;
        private ILogger<PaymentService> _logger;
        private PaymentReadRepository _paymentReadRepository;
        private IMapper _mapper;

        public virtual async Task<Guid> ProcessPaymentAsync(PaymentDto dto)
        {
            dto.Id = Guid.NewGuid();
            string paymentSerialized = JsonSerializer.Serialize(dto);

            _logger.LogInformation($"Sending payment with id {dto.Id} to messaging service.");
            await _messagingService.SendPaymentAsync(paymentSerialized);
            return dto.Id;
        }

        public async virtual Task<PaymentDetailsDto> GetPaymentDetailsAsync(Guid guid, Guid userId)
        {
            var paymentRead = await _paymentReadRepository.GetPaymentDetailsAsync(guid, userId);
            return _mapper.Map<PaymentDetailsDto>(paymentRead);
        }

        public PaymentService(RabbitMqPublisher messagingService, ILogger<PaymentService> logger, PaymentReadRepository paymentReadRepository, IMapper mapper)
        {
            _messagingService = messagingService;
            _logger = logger;
            _paymentReadRepository = paymentReadRepository;
            _mapper = mapper;
        }

        protected PaymentService()
        {

        }
    }
}
