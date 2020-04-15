using MediatR;
using PaymentGatewayWorker.Domain.Payments.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayWorker.CQRS.CommandStack.Events
{
    class UpdatePaymentStatusWithBankResponseCommand : IRequest
    {
        public UpdatePaymentStatusWithBankResponseCommand(PaymentHubResponse response)
        {
            Response = response;
        }

        public PaymentHubResponse Response { get; set; }
    }
}
