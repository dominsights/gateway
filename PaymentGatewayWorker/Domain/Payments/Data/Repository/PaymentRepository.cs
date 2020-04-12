using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;

using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using PaymentGatewayWorker.Domain.Payments.Interfaces;

using CQRS;
using Entities = PaymentGatewayWorker.Domain.Payments.Data.Entities;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace PaymentGatewayWorker.Domain.Payments.Data.Repository
{
    class PaymentRepository : IRepository, IPaymentRepository
    {
        private IMongoCollection<PaymentReadModel> _paymentsRead;
        private PaymentsDbContext _paymentsDbContext;
        private IMapper _mapper;
        private ILogger<PaymentRepository> _logger;

        // IPaymentRepository

        public async Task AddAsync(Payment payment)
        {
            try
            {
                var entity = _mapper.Map<Entities.Payment>(payment);
                await _paymentsDbContext.AddAsync(entity);
                await _paymentsDbContext.SaveChangesAsync();

                var paymentRead = _mapper.Map<PaymentReadModel>(payment);
                paymentRead.Status = PaymentStatus.CREATED;
                await _paymentsRead.InsertOneAsync(paymentRead);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to add payment to database.");
                throw;
            }
        }

        public async Task<Payment> GetByIdAsync(Guid id)
        {
            var entity = await _paymentsDbContext.Payments.FindAsync(id);
            return _mapper.Map<Payment>(entity);
        }

        public Task<Payment> GetByFilterAsync(PaymentFilter paymentFilter)
        {
            Task<Payment> task = new Task<Payment>(() =>
            {
                var entity = _paymentsDbContext.Payments.FirstOrDefault(p =>
                   p.Amount == paymentFilter.Amount &&
                   p.CardNumber == paymentFilter.CardNumber &&
                   p.CurrencyCode == paymentFilter.CurrencyCode &&
                   p.CVV == paymentFilter.CVV);

                return _mapper.Map<Payment>(entity);
            });

            task.Start();

            return task;
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

        public PaymentRepository(MongoDbSettings mongoDbSettings, PaymentsDbContext paymentsDbContext, IMapper mapper, ILogger<PaymentRepository> logger)
        {
            var client = new MongoClient(mongoDbSettings.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);
            _paymentsRead = database.GetCollection<PaymentReadModel>(mongoDbSettings.PaymentsCollectionName);
            _paymentsDbContext = paymentsDbContext;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
