using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Authorization.Data
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private UserAccountDbContext _context;

        public UserAccountRepository(UserAccountDbContext context)
        {
            _context = context;
        }


        public async Task SaveAsync(UserAccount userAccount)
        {
            await _context.Users.AddAsync(userAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<UserAccount> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstAsync(u => u.Username == username);
        }
    }
}
