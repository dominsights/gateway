using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbRepository
{
    public class PaymentReadRepository
    {
        private IMongoCollection<PaymentReadModel> _paymentsRead;

        public virtual async Task UpdatePaymentReadModelStatusAsync(Guid aggregateId, PaymentStatus paymentStatus)
        {
            var filter = Builders<PaymentReadModel>.Filter.Eq(p => p.Id, aggregateId);
            var update = Builders<PaymentReadModel>.Update.Set(p => p.Status, paymentStatus);

            await _paymentsRead.UpdateOneAsync(filter, update);
        }

        public async Task InsertOneAsync(PaymentReadModel paymentRead)
        {
            await _paymentsRead.InsertOneAsync(paymentRead);
        }

        public async virtual Task<PaymentReadModel> GetPaymentDetailsAsync(Guid aggregateId, Guid userId)
        {
            FilterDefinitionBuilder<PaymentReadModel> builder = Builders<PaymentReadModel>.Filter;
            var filter = builder.Eq(p => p.Id, aggregateId) & builder.Eq(p => p.UserId, userId);

            return await _paymentsRead.Find(filter).FirstAsync();
        }

        public PaymentReadRepository(MongoDbSettings mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _paymentsRead = database.GetCollection<PaymentReadModel>(mongoDbSettings.PaymentsCollectionName);
        }

        // Necessary for mocking
        protected PaymentReadRepository()
        {

        }
    }
}
