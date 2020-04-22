using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDbRepository;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using PaymentGatewayWorker.Domain.Payments.Data.Repository;
using PaymentGatewayWorker.Domain.Payments.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.CQRS.CommandStack.Handlers
{
    class PaymentCreatedEventHandler : INotificationHandler<PaymentCreatedEvent>
    {
        private BankResponseRepository _bankResponseRepository;
        private IMediator _mediator;
        private ILogger<PaymentCreatedEventHandler> _logger;
        private BankService _bankService;
        private PaymentRepository _paymentRepository;
        private IMapper _mapper;

        public async Task Handle(PaymentCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Domain.Payments.Payment payment = _mapper.Map<Domain.Payments.Payment>(notification);
                var response = await _bankService.SendPaymentForBankApprovalAsync(payment);

                var bankResponse = new BankResponse
                {
                    Id = response,
                    PaymentId = notification.AggregateId
                };

                await _bankResponseRepository.SaveBankResponseAsync(bankResponse);
                await _paymentRepository.UpdatePaymentReadModelStatusAsync(notification.AggregateId, PaymentStatus.PROCESSING);

                var sentToBankEvent = new PaymentSentForBankApprovalEvent(notification.AggregateId, bankResponse.Id);
                await _mediator.Publish(sentToBankEvent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to send payment to bank.");
                var errorEvent = new SendPaymentForBankApprovalErrorEvent();
                await _mediator.Send(errorEvent);
            }
        }

        public PaymentCreatedEventHandler(BankResponseRepository bankResponseRepository, IMediator mediator, ILogger<PaymentCreatedEventHandler> logger, BankService bankService, PaymentRepository paymentRepository, IMapper mapper)
        {
            _bankResponseRepository = bankResponseRepository;
            _mediator = mediator;
            _logger = logger;
            _bankService = bankService;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }
    }
}
