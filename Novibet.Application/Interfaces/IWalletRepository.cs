using Novibet.Domain.Entities;

namespace Novibet.Application.Interfaces
{
    public interface IWalletRepository
    {
        Task<long> CreateAsync(Wallet wallet);
        Task<Wallet?> RetrieveWalletByIdAsync(long id);
        Task UpdateAsync(Wallet wallet);
    }
}
