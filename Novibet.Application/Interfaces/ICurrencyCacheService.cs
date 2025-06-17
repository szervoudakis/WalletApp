namespace Novibet.Application.Interfaces
{
    public interface ICurrencyCacheService
    {
        Task<Dictionary<string, decimal>> GetCachedCurrencyRatesAsync();
        Task SetCurrencyRatesAsync(Dictionary<string, decimal> rates);

        Task<string> PingRedisAsync();
         
         Task<decimal?> GetCachedRateAsync(string fromCurrency, string toCurrency);

    }
}
