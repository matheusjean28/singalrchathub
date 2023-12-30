using Microsoft.EntityFrameworkCore;

namespace UserContext
{
    public class UserDbContext : DbContext
    {
        public DbSet<UserModel.User> Users { get; set; }

         public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}
