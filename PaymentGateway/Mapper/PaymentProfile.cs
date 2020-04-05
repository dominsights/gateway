using AutoMapper;
using PaymentGateway.Model;
using PaymentGateway.Payments.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Mapper
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentInput, PaymentDto>();
        }
    }
}
