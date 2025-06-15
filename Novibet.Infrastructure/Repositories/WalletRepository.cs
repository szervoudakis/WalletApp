using Novibet.Domain.Entities;
using Novibet.Application.Interfaces;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Novibet.Infrastructure.Data;

namespace Novibet.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
         private readonly NovibetDbContext _context;
        public WalletRepository(NovibetDbContext context)
        {
            _context = context;
        }
        public async Task<long> CreateAsync(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();

            return wallet.Id;//after saving return id
        }
    }
}