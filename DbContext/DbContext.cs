using ChatModel;
using ChatSignalR.Models.PermisionsChat;
using Microsoft.EntityFrameworkCore;
using UserModel;

namespace UserContext
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UserPermissionData> UserPermission { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Chat>()
                .HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
