using paymatesapi.Models;
using Microsoft.EntityFrameworkCore;

namespace paymatesapi.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
    }
}