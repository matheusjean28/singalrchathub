using Microsoft.EntityFrameworkCore;
using UserModel;

namespace UserContext
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

         public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}
