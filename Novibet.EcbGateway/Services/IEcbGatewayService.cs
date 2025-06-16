using Novibet.EcbGateway.Models;

namespace Novibet.EcbGateway.Services
{
    public interface IEcbGatewayService
    {
        Task<List<CurrencyRate>> GetLatestRatesAsync();
    }
}