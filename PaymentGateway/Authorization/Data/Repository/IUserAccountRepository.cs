using System.Threading.Tasks;

namespace PaymentGateway.Authorization.Data
{
    public interface IUserAccountRepository
    {
        Task SaveAsync(UserAccount userAccount);
        Task<UserAccount> GetByUsernameAsync(string username);
    }
}