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
using System.Runtime.CompilerServices;

namespace PaymentGatewayWorker.CQRS.CommandStack.Sagas
{
    class UpdatePaymentStatusWithBankResponseHandler : IRequestHandler<UpdatePaymentStatusWithBankResponseCommand>
    {
        private ILogger<UpdatePaymentStatusWithBankResponseHandler> _logger;
        private IMediator _mediator;
        private PaymentService _paymentService;

        public async Task<Unit> Handle(UpdatePaymentStatusWithBankResponseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await _paymentService.ValidateToUpdateStatusAsync(request.Response);

                if (payment.ValidationResult.IsValid)
                {
                    var acceptedEvent = new PaymentAcceptedEvent(payment);
                    await _mediator.Publish(acceptedEvent);
                }
                else
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

        public UpdatePaymentStatusWithBankResponseHandler(IMediator mediator, ILogger<UpdatePaymentStatusWithBankResponseHandler> logger, PaymentService paymentService)
        {
            _logger = logger;
            _mediator = mediator;
            _paymentService = paymentService;
        }
    }
}
