using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGatewayWorker.CQRS.CommandStack.Events
{
    public class PaymentCreatedEvent : Event, INotification
    {
        public PaymentCreatedEvent(Guid aggregateId, Guid userId, string cardNumber, int expiryMonth, int expiryYear, decimal amount, string currencyCode, string cvv) : base()
        {
            AggregateId = aggregateId;
            UserId = userId;
            CardNumber = cardNumber;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Amount = amount;
            CurrencyCode = currencyCode;
            CVV = cvv;
        }

        public Guid UserId { get; set; }
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string CVV { get; set; }
    }
}
