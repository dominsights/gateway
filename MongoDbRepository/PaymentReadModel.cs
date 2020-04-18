using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDbRepository
{
    public class PaymentReadModel
    {
        [BsonId]
        public Guid Id { get; set; }
        [BsonRequired]
        public Guid UserId { get; set; }
        [BsonRequired]
        public string CardNumber { get; set; }
        [BsonRequired]
        public decimal Amount { get; set; }
        [BsonRequired]
        public string CurrencyCode { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonRequired]
        public PaymentStatus Status { get; set; }
    }
}
