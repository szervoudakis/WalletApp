using WalletApp.Application.DTOs;

namespace WalletApp.Application.Interfaces
{
    public interface IEcbService
    {
        Task<List<CurrencyRateDto>> GetLatestRatesAsync();
    }

}
