using AutoMapper;
using PaymentGateway.Mapper;
using PaymentGateway.Payments.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayUnitTests
{
    public class PaymentGatewayFixture
    {
        public IMapper Mapper { get; set; }
        public PaymentInput PaymentInput { get; internal set; }

        public PaymentGatewayFixture()
        {
            var mapperConfig = MapperConfigurationFactory.MapperConfiguration;

            Mapper = mapperConfig.CreateMapper();

            PaymentInput = new PaymentInput()
            {
                Amount = 10.50m,
                CardNumber = "4532517464958844",
                CurrencyCode = "EUR",
                CVV = "078",
                ExpiryMonth = 10,
                ExpiryYear = 23
            };
        }
    }
}
