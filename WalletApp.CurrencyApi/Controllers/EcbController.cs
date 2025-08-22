using Microsoft.AspNetCore.Mvc;
using WalletApp.EcbGateway.Services;
using WalletApp.Domain.Entities;
using WalletApp.Infrastructure.Repositories;
using WalletApp.Infrastructure.Services;
using WalletApp.Application.Interfaces;

namespace WalletApp.CurrencyApi.Controllers
{
    [ApiController]
    [Route("api/ecb")]
    public class EcbController : ControllerBase
    {
        private readonly IEcbGatewayService _ecbService;
        private readonly ICurrencyService _currencyService;
        public EcbController(IEcbGatewayService ecbService, ICurrencyService currencyRepository, CurrencyService currencyService)
        {
            _ecbService = ecbService;
            _currencyService = currencyService;
        }
        
        [HttpGet("rates")]
        public async Task<IActionResult> GetRates()
        {
            var rates = await _currencyService.GetCurrencyRatesAsync();
            return Ok(rates);
        }
    }
}