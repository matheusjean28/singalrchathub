using Microsoft.EntityFrameworkCore;

namespace UserContext
{
    public class UserContext : DbContext
    {
        public DbSet<UserModel.User> Users { get; set; }

    }
    
}
