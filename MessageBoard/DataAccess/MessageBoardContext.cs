using MessageBoard.Models;
using MessageBoard.Services;
using Microsoft.EntityFrameworkCore;

namespace MessageBoard.DataAccess
{
    public class MessageBoardContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public MessageBoardContext(DbContextOptions<MessageBoardContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Email = "admin@messageboard.com", Name = "Admin", Role = Role.Admin, PasswordDigest = HashService.Digest("admin-password") });
        }
    }
}
