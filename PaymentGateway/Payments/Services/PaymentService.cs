using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentGateway.Payments.Services
{
    public class PaymentService : IPaymentService
    {
        private IMessagingService _messagingService;
        private ILogger<PaymentService> _logger;

        public PaymentService(IMessagingService messagingService, ILogger<PaymentService> logger)
        {
            _messagingService = messagingService;
            _logger = logger;
        }

        public async Task<Guid> ProcessPaymentAsync(PaymentDto dto)
        {
            dto.Id = Guid.NewGuid();
            string paymentSerialized = JsonSerializer.Serialize(dto);

            _logger.LogInformation($"Sending payment with id {dto.Id} to messaging service.");
            await _messagingService.SendPaymentAsync(paymentSerialized);
            return dto.Id;
        }
    }
}
