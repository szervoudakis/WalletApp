using Microsoft.EntityFrameworkCore;
using Novibet.Domain.Entities;
using EFCore.BulkExtensions;

namespace Novibet.Infrastructure.Data
{
    public class NovibetDbContext : DbContext
    {
        public NovibetDbContext(DbContextOptions<NovibetDbContext> options) : base(options) { }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<User> Users { get; set; } = null!;
    }
}