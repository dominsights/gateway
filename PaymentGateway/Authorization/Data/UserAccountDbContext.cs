using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Authorization.Data
{
    public class UserAccountDbContext : DbContext
    {
        public UserAccountDbContext(DbContextOptions<UserAccountDbContext> options) : base(options)
        {
        }

        public DbSet<UserAccount> Users { get; set; }
    }
}
