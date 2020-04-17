using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Mapper;
using PaymentGateway.Payments.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace PaymentGatewayUnitTests
{
    public class PaymentGatewayFixture
    {
        public IMapper Mapper { get; set; }
        public PaymentInput PaymentInput { get; internal set; }
        public ControllerContext ControllerContext
        {
            get
            {
                // Prepare logged user
                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, Guid.NewGuid().ToString())
            };

                var identity = new ClaimsIdentity(claims, "test");

                // Insert logged user in the controller context
                var context = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(identity)
                    }
                };
                return context;
            }
        }

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
