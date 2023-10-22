using paymatesapi.Entities;
using Microsoft.EntityFrameworkCore;

namespace paymatesapi.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Friend> Friends { get; set; }
    }
}