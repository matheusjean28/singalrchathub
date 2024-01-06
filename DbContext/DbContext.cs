using Microsoft.EntityFrameworkCore;
using ChatModel;
using UserModel;

namespace UserContext
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }

         public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}
