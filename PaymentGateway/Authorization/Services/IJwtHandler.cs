using Microsoft.IdentityModel.Tokens;

namespace PaymentGateway.Authorization.Services
{
    public interface IJwtHandler
    {
        TokenValidationParameters Parameters { get; }

        JWT Create(string userId);
    }
}