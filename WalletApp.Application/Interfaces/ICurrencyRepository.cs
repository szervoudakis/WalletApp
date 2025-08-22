using WalletApp.Domain.Entities;

namespace WalletApp.Application.Interfaces
{
    public interface ICurrencyRepository
    {
        Task SaveCurrencyRatesAsync(List<Currency> currencies);
        
        Task<List<Currency>> GetLatestRatesAsync();

        Task UpdateAsync(Currency currency);

        Task<Dictionary<string,decimal>> RetrieveCurrencyRateAsync(string currency);
    }
}
