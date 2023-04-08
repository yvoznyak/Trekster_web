using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<StartBalance> StartBalances { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Currency>()
            .HasData(
                new Currency { Id = 1, Name = "Uah" },
                new Currency { Id = 2, Name = "Usd" },
                new Currency { Id = 3, Name = "Eur" },
                new Currency { Id = 4, Name = "Usdt" },
                new Currency { Id = 5, Name = "Btc" }
            );
        }
    }
}
