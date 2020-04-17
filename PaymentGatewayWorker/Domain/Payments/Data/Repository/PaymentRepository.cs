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
using Microsoft.EntityFrameworkCore;

namespace PaymentGatewayWorker.Domain.Payments.Data.Repository
{
    class PaymentRepository : IRepository, IPaymentRepository
    {
        private IMongoCollection<PaymentReadModel> _paymentsRead;
        private PaymentsDbContext _paymentsDbContext;
        private IMapper _mapper;
        private ILogger<PaymentRepository> _logger;

        // IPaymentRepository

        private async Task AddAsync(Payment payment)
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

        internal virtual async Task<Payment> GetByBankResponseIdAsync(Guid id)
        {
            var query = from p in _paymentsDbContext.Payments
                        join b in _paymentsDbContext.BankResponses
                        on p.Id equals b.PaymentId
                        where b.Id == id
                        select p;

            var entity = await query.FirstAsync();
            Payment payment = _mapper.Map<Payment>(entity);

            return payment;
        }

        public virtual async Task<Payment> GetByIdAsync(Guid id)
        {
            var entity = await _paymentsDbContext.Payments.FindAsync(id);
            return _mapper.Map<Payment>(entity);
        }

        public virtual Task<Payment> GetByFilterAsync(PaymentFilter paymentFilter)
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

        public async Task<CommandResponse> CreateFromRequestAsync<T>(T item) where T : class
        {
            var request = item as Payment;

            try
            {
                await AddAsync(request);
                var response = new CommandResponse(true, request.Id);
                return response;
            } catch(Exception e)
            {
                _logger.LogError(e, $"Error while trying to create from request payment id {request.Id}");
                return new CommandResponse(false, request.Id);
            }
        }

        public virtual async Task UpdatePaymentReadModelStatusAsync(Guid aggregateId, PaymentStatus paymentStatus)
        {
            var filter = Builders<PaymentReadModel>.Filter.Eq(p => p.Id, aggregateId);
            var update = Builders<PaymentReadModel>.Update.Set(p => p.Status, paymentStatus);

            await _paymentsRead.UpdateOneAsync(filter, update);
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

        // Necessary for mocking
        protected PaymentRepository()
        {

        }
    }
}
