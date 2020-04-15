using CQRS;
using MediatR;
using PaymentGatewayWorker.CQRS.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.CQRS.CommandStack.Handlers
{
    class AddNewPaymentHandler : IRequestHandler<AddNewPaymentCommand>
    {
        private IRepository _repository;
        private IMediator _mediator;

        public async Task<Unit> Handle(AddNewPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = Domain.Payments.Payment.Factory.NewPayment(request.AggregateId, request.UserId, request.CardNumber,
                request.ExpiryMonth, request.ExpiryYear, request.Amount, request.CurrencyCode, request.CVV);

            var response = await _repository.CreateFromRequestAsync(payment);

            if (!response.Success)
            {
                var errorEvent = new AddNewPaymentErrorEvent();
                await _mediator.Send(errorEvent);
            }

            var createdEvent = new PaymentCreatedEvent(request.AggregateId, payment);
            await _mediator.Publish(createdEvent);
            return Unit.Value;
        }

        public AddNewPaymentHandler(IRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }
    }
}
