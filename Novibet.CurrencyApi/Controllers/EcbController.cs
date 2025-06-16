using Microsoft.AspNetCore.Mvc;
using Novibet.EcbGateway.Services;
using Novibet.Domain.Entities;
using Novibet.Infrastructure.Repositories;
using Novibet.Infrastructure.Services;

namespace Novibet.CurrencyApi.Controllers
{
    [ApiController]
    [Route("api/ecb")]
    public class EcbController : ControllerBase
    {
        private readonly IEcbGatewayService _ecbService;
        private readonly CurrencyService _currencyService;
        public EcbController(IEcbGatewayService ecbService, CurrencyRepository currencyRepository, CurrencyService currencyService)
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