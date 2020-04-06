using System.Threading.Tasks;
using PaymentGateway.Authorization.Data;

namespace PaymentGateway.Authorization.Services
{
    public interface IAuthService
    {
        Task<UserJwt> LoginAsync(string userName, string password);
        Task<UserAccount> SaveAsync(string username, string password);
    }
}