namespace Novibet.Application.Interfaces
{
    public interface ICurrencyService
    {
        Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency);
        Task UpdateCurrencyRatesAsync();
        Task<Dictionary<string, decimal>> GetCurrencyRatesAsync();
    }
}
