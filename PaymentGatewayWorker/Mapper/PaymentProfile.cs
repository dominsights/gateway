using AutoMapper;
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
            CreateMap<PaymentDto, Payment>();
            CreateMap<Payment, Entities.Payment>();
            CreateMap<Payment, Entities.PaymentReadModel>();
        }
    }
}
