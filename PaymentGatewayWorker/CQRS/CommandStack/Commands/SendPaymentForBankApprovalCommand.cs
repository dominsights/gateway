using CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.CQRS.CommandStack.Commands
{
    public class SendPaymentForBankApprovalCommand : Command
    {
        public Guid UserId { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string CVV { get; set; }

        public SendPaymentForBankApprovalCommand(Guid aggregateId, Guid userId, string cardNumber, int expiryMonth, int expiryYear, decimal amount, string currencyCode, string cvv)
        {
            Name = "SendPaymentForBankApproval";
            UserId = userId;
            AggregateId = aggregateId;
            CardNumber = cardNumber;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Amount = amount;
            CurrencyCode = currencyCode;
            CVV = cvv;
        }
    }
}
