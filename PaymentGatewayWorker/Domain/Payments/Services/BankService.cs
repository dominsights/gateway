using CQRS;
using PaymentGatewayWorker.CQRS.CommandStack.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayWorker.Domain.Payments.Services
{
    public class BankService
    {
        public Task<CommandResponse> SendPaymentForBankApprovalAsync(SendPaymentForBankApprovalCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
