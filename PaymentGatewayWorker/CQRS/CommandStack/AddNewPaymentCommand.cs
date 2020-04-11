using CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.CQRS
{
    class AddNewPaymentCommand : Command
    {
        public Guid UserId { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string CVV { get; set; }

        public AddNewPaymentCommand(Guid userId, Guid id, string cardNumber, int expiryMonth, int expiryYear, decimal amount, string currencyCode, string cvv)
        {
            Name = "AddNewPayment";
            UserId = userId;
            AggregateId = id;
            CardNumber = cardNumber;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Amount = amount;
            CurrencyCode = currencyCode;
            CVV = cvv;
        }
    }
}
