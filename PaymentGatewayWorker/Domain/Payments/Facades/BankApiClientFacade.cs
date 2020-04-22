using Microsoft.Extensions.Options;
using PaymentGatewayWorker.BankApi;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.Domain.Payments.Facades
{
    class BankApiClientFacade
    {
        private BankApiClient _bankApiClient;

        public virtual async Task<Guid> SendPaymentToBankAsync(Payment payment)
        {
            var body = new PaymentGatewayWorker.BankApi.Payment
            {
                Amount = (double)payment.Amount,
                CardNumber = payment.CardNumber,
                CurrencyCode = payment.CurrencyCode,
                Cvv = payment.CVV,
                ExpiryMonth = payment.ExpiryMonth,
                ExpiryYear = payment.ExpiryYear,
                SellerId = payment.Id
            };

            return await _bankApiClient.PaymentAsync(body);
        }

        public BankApiClientFacade(IOptions<BankAPIConfig> signalRConfig)
        {
            var httpClient = new HttpClient();
           _bankApiClient = new BankApiClient(signalRConfig.Value.ServerUrl, httpClient);
        }

        // Necessary for mocking
        protected BankApiClientFacade()
        {

        }
    }
}
