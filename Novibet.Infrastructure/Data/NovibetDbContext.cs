using Microsoft.EntityFrameworkCore;
using Novibet.Domain.Entities;

namespace Novibet.Infrastructure.Data
{
    public class NovibetDbContext : DbContext
    {
        public NovibetDbContext(DbContextOptions<NovibetDbContext> options) : base(options) { }

        public DbSet<Currency> Currencies { get; set; }
        
    }
}