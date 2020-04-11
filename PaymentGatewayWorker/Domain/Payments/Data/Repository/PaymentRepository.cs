using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CQRS;
using MongoDB.Driver;
using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using PaymentGatewayWorker.Domain.Payments.Interfaces;

namespace PaymentGatewayWorker.Domain.Payments.Data.Repository
{
    class PaymentRepository : IRepository, IPaymentRepository
    {
        private IMongoCollection<PaymentReadModel> _paymentsRead;

        // IPaymentRepository

        public Task AddAsync(Payment payment)
        {
            throw new NotImplementedException();
        }

        public Task<Payment> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Payment> GetByFilterAsync(PaymentFilter paymentFilter)
        {
            throw new NotImplementedException();
        }

        // IRepository

        public T GetById<T>(int id) where T : class
        {
            throw new NotImplementedException();
        }

        public CommandResponse CreateFromRequest<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public CommandResponse Update<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public PaymentRepository(MongoDbSettings mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _paymentsRead = database.GetCollection<PaymentReadModel>(mongoDbSettings.PaymentsCollectionName);
        }
    }
}
