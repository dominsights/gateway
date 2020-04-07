using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace PaymentGateway.Authorization.Services
{
    public class JwtHandler : IDisposable, IJwtHandler
    {
        private readonly JwtSettings _settings;
        private SecurityKey _issuerSigningKey;
        private SigningCredentials _signingCredentials;
        public TokenValidationParameters Parameters { get; private set; }

        private RSA _publicRsa;
        private RSA _privateRsa;

        public JwtHandler(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
            InitializeRsa();
            InitializeJwtParameters();
        }

        private void InitializeRsa()
        {
            _publicRsa = RSA.Create();
            var publicKeyXml = File.ReadAllText(_settings.PublicKeyXML);
            _publicRsa.RsaFromXmlString(publicKeyXml);
            _issuerSigningKey = new RsaSecurityKey(_publicRsa);

            if (string.IsNullOrWhiteSpace(_settings.PrivateKeyXML))
            {
                return;
            }

            _privateRsa = RSA.Create();

            var privateKeyXml = File.ReadAllText(_settings.PrivateKeyXML);
            _privateRsa.RsaFromXmlString(privateKeyXml);
            var privateKey = new RsaSecurityKey(_privateRsa);
            _signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
        }

        private void InitializeJwtParameters()
        {
            Parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidIssuer = _settings.Issuer,
                IssuerSigningKey = _issuerSigningKey
            };
        }

        public JWT Create(string userId)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddDays(2);
            var centuryBegin = new DateTime(1970, 1, 1);
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var issuer = _settings.Issuer ?? string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                IssuedAt = nowUtc,
                Expires = expires,
                NotBefore = nowUtc,
                Issuer = issuer,
                SigningCredentials = _signingCredentials
            };

            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(jwt);

            return new JWT
            {
                Token = token,
                Expires = exp
            };
        }

        public void Dispose()
        {
            _publicRsa?.Dispose();
            _privateRsa?.Dispose();
        }
    }

    static class JwtExtensions
    {
        public static void RsaFromXmlString(this RSA rsa, string xmlString)
        {
            var parameters = new RSAParameters();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }
    }
}
