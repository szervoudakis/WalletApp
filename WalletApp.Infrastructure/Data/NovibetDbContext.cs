using Microsoft.EntityFrameworkCore;
using WalletApp.Domain.Entities;
using EFCore.BulkExtensions;

namespace WalletApp.Infrastructure.Data
{
    public class NovibetDbContext : DbContext
    {
        public NovibetDbContext(DbContextOptions<NovibetDbContext> options) : base(options) { }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<User> Users { get; set; } = null!;
    }
}