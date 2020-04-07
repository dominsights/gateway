using Microsoft.Extensions.Options;
using Moq;
using PaymentGateway.Authorization;
using PaymentGateway.Authorization.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Xunit;

namespace PaymentGatewayUnitTests
{
    public class JwtHandlerTests
    {
        [Fact]
        public void ShouldCreateValidTokenWhenKeysAreValid()
        {
            var jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
            var jwtSettings = new JwtSettings();
            jwtSettings.Issuer = "http://localhost:5000";
            jwtSettings.PrivateKeyXML = "private-key.xml";
            jwtSettings.PublicKeyXML = "public-key.xml";

            jwtSettingsMock.Setup(j => j.Value).Returns(jwtSettings);

            var jwtHandler = new JwtHandler(jwtSettingsMock.Object);
            var token = jwtHandler.Create(Guid.NewGuid().ToString());

            var tokenHandler = new JwtSecurityTokenHandler();
            var exception = Record.Exception(() => tokenHandler.ValidateToken(token.Token, jwtHandler.Parameters, out _));

            Assert.Null(exception);
        }
    }
}
