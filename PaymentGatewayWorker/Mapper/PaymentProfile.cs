using AutoMapper;
using PaymentGatewayWorker.CQRS.CommandStack.Commands;
using PaymentGatewayWorker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Entities =  PaymentGatewayWorker.Domain.Payments.Data.Entities;

namespace PaymentGatewayWorker.Mapper
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentDto, Domain.Payments.Payment>();
            CreateMap<Domain.Payments.Payment, Entities.Payment>();
            CreateMap<Domain.Payments.Payment, Entities.PaymentReadModel>();
            CreateMap<Entities.Payment, Domain.Payments.Payment>();
            CreateMap<SendPaymentForBankApprovalCommand, Domain.Payments.Payment>();
        }
    }
}
