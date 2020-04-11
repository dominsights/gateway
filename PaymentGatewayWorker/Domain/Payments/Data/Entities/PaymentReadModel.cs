using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.Domain.Payments.Data.Entities
{
    class PaymentReadModel
    {
        // add status - CREATED, APPROVED, DENIED etc
        // add data to check payment details

        [BsonId]
        public Guid Id { get; set; }
        [BsonRequired]
        public Guid UserId { get; set; }
        [BsonRequired]
        public string CardNumber { get; set; }
        [BsonRequired]
        public int ExpiryMonth { get; set; }
        [BsonRequired]
        public int ExpiryYear { get; set; }
        [BsonRequired]
        public decimal Amount { get; set; }
        [BsonRequired]
        public string CurrencyCode { get; set; }
        [BsonRequired]
        public string CVV { get; set; }
    }
}
