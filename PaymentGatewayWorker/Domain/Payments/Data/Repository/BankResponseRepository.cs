using Microsoft.Extensions.Logging;
using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.Domain.Payments.Data.Repository
{
    class BankResponseRepository
    {
        private PaymentsDbContext _paymentsDbContext;
        private ILogger<BankResponseRepository> _logger;

        public async Task SaveBankResponseAsync(BankResponse bankResponse)
        {
            await _paymentsDbContext.BankResponses.AddAsync(bankResponse);
            await _paymentsDbContext.SaveChangesAsync();
        }

        public BankResponseRepository(PaymentsDbContext paymentsDbContext, ILogger<BankResponseRepository> logger)
        {
            _paymentsDbContext = paymentsDbContext;
            _logger = logger;
        }
    }
}
