using Novibet.Domain.Entities;

namespace Novibet.Application.Interfaces
{
    public interface ICurrencyRepository
    {
        Task SaveCurrencyRatesAsync(List<Currency> currencies);
        Task<List<Currency>> GetLatestRatesAsync();
    }
}
