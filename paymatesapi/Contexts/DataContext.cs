using paymatesapi.Entities;
using Microsoft.EntityFrameworkCore;

namespace paymatesapi.Contexts
{
    public class DataContext : DbContext
    {
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Friend>()
        //         .HasMany(f => f.Transactions)
        //         .WithOne(t => t.FriendPair)
        //         .OnDelete(DeleteBehavior.Cascade);

        //     modelBuilder.Entity<Friend>()
        //         .Navigation(f => f.Transactions)
        //         .UsePropertyAccessMode(PropertyAccessMode.Property);

        //     base.OnModelCreating(modelBuilder);
        // }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}