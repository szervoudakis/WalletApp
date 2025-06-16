using Novibet.Application.DTOs;

namespace Novibet.Application.Interfaces
{
    public interface IEcbService
    {
        Task<List<CurrencyRateDto>> GetLatestRatesAsync();
    }

}
