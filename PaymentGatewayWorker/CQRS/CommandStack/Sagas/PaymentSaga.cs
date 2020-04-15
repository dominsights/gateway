using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

using CQRS;
using PaymentGatewayWorker.Domain.Payments.Services;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using MediatR;
using System.Threading;
using PaymentGatewayWorker.EventSourcing;
using Microsoft.AspNetCore.SignalR.Client;
using PaymentGatewayWorker.Domain.Payments.Data.Repository;
using PaymentGatewayWorker.Domain.Payments.Data.Entities;
using PaymentGatewayWorker.Domain.Services;

namespace PaymentGatewayWorker.CQRS.CommandStack.Sagas
{
    class PaymentSaga :
        IRequestHandler<AddNewPaymentCommand>,
        INotificationHandler<PaymentCreatedEvent>,
        IRequestHandler<UpdatePaymentStatusWithBankResponseCommand>
    {
        private ILogger<PaymentSaga> _logger;
        private IRepository _respository;
        private IMediator _mediator;
        private BankService _bankService;
        private BankResponseRepository _bankResponseRepository;
        private PaymentService _paymentService;

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

        public async Task<Unit> Handle(UpdatePaymentStatusWithBankResponseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await _paymentService.ValidateToUpdateStatusAsync(request.Response);

                if (payment.ValidationResult.IsValid)
                {
                    var acceptedEvent = new PaymentAcceptedEvent(payment);
                    await _mediator.Publish(acceptedEvent);
                } else
                {
                    _logger.LogWarning($"Payment with id: {payment.Id} validated by bank. Can't validate again.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured while trying to update payment status from bank response.");
            }

            return Unit.Value;
        }

        public PaymentSaga(IMediator mediator, IEventStore eventStore, IRepository repository, BankService bankService, ILogger<PaymentSaga> logger, BankResponseRepository bankResponseRepository, PaymentService paymentService)
        {
            _logger = logger;
            _respository = repository;
            _mediator = mediator;
            _bankService = bankService;
            _bankResponseRepository = bankResponseRepository;
            _paymentService = paymentService;
        }
    }
}
