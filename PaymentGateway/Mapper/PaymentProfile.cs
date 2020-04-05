using AutoMapper;
using PaymentGateway.Payments.Models;
using PaymentGateway.Payments.Services;
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
