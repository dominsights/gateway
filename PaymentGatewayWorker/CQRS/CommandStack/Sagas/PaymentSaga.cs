using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using CQRS;
using PaymentGatewayWorker.Domain.Payments.Services;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using MediatR;
using System.Threading;
using PaymentGatewayWorker.EventSourcing;

namespace PaymentGatewayWorker.CQRS.CommandStack.Sagas
{
    class PaymentSaga :
        IRequestHandler<AddNewPaymentCommand>,
        INotificationHandler<PaymentCreatedEvent>
    {
        private ILogger<PaymentSaga> _logger;
        private IRepository _respository;
        private IMediator _mediator;
        private BankService _bankService;

        public async Task<Unit> Handle(AddNewPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = Domain.Payments.Payment.Factory.NewPayment(request.AggregateId, request.UserId, request.CardNumber,
                request.ExpiryMonth, request.ExpiryYear, request.Amount, request.CurrencyCode, request.CVV);

            var response = await _respository.CreateFromRequestAsync(payment);

            if (!response.Success)
            {
                var errorEvent = new AddNewPaymentErrorEvent();
                await _mediator.Send(errorEvent);
            }

            var createdEvent = new PaymentCreatedEvent(request.AggregateId, payment);
            await _mediator.Publish(createdEvent);
            return Unit.Value;
        }

        public async Task Handle(PaymentCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _bankService.SendPaymentForBankApprovalAsync(notification.Data);

                var sentToBankEvent = new PaymentSentForBankApprovalEvent(notification.AggregateId, notification.Data);
                await _mediator.Send(sentToBankEvent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while trying to send payment to bank.");
                var errorEvent = new SendPaymentForBankApprovalErrorEvent();
                await _mediator.Send(errorEvent);
            }
        }

        public PaymentSaga(IMediator mediator, IEventStore eventStore, IRepository repository, BankService bankService, ILogger<PaymentSaga> logger)
        {
            _logger = logger;
            _respository = repository;
            _mediator = mediator;
            _bankService = bankService;
        }
    }
}
