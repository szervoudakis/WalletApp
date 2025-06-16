namespace Novibet.Application.Interfaces
{
    public interface ICurrencyCacheService
    {
        Task<Dictionary<string, decimal>> GetCachedCurrencyRatesAsync();
        Task SetCurrencyRatesAsync(Dictionary<string, decimal> rates);
    }
}
