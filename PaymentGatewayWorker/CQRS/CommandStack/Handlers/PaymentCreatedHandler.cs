using MediatR;
using Microsoft.Extensions.Logging;
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
    class PaymentCreatedHandler : INotificationHandler<PaymentCreatedEvent>
    {
        private BankResponseRepository _bankResponseRepository;
        private IMediator _mediator;
        private ILogger<PaymentCreatedHandler> _logger;
        private BankService _bankService;

        public async Task Handle(PaymentCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _bankService.SendPaymentForBankApprovalAsync(notification.Data);

                var bankResponse = new BankResponse
                {
                    Id = response,
                    PaymentId = notification.Data.Id
                };

                await _bankResponseRepository.SaveBankResponseAsync(bankResponse);

                var sentToBankEvent = new PaymentSentForBankApprovalEvent(notification.AggregateId, notification.Data);
                await _mediator.Publish(sentToBankEvent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to send payment to bank.");
                var errorEvent = new SendPaymentForBankApprovalErrorEvent();
                await _mediator.Send(errorEvent);
            }
        }

        public PaymentCreatedHandler(BankResponseRepository bankResponseRepository, IMediator mediator, ILogger<PaymentCreatedHandler> logger, BankService bankService)
        {
            _bankResponseRepository = bankResponseRepository;
            _mediator = mediator;
            _logger = logger;
            _bankService = bankService;
        }
    }
}
