using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Domain
{
    public interface IUserAccountRepository
    {
        Task SaveAsync(UserAccount userAccount);
        Task<UserAccount> GetByUsernameAsync(string username);
    }
}