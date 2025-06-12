using Novibet.EcbGateway.Models;

namespace Novibet.EcbGateway.Services
{
    public interface IEcbService
    {
        Task<List<CurrencyRate>> GetLatestRatesAsync();
    }
}