using PaymentGateway.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PaymentGatewayUnitTests
{
    public class MapperTests
    {
        [Fact]
        public void ValidateMapperProfiles()
        {
            var configuration = MapperConfigurationFactory.MapperConfiguration;
            configuration.AssertConfigurationIsValid();
        }
    }
}
