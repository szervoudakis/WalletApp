using WalletApp.EcbGateway.Models;

namespace WalletApp.EcbGateway.Services
{
    public interface IEcbGatewayService
    {
        Task<List<CurrencyRate>> GetLatestRatesAsync();
    }
}