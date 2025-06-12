using Microsoft.AspNetCore.Mvc;
using Novibet.EcbGateway.Services;

namespace Novibet.CurrencyApi.Controllers
{
    [ApiController]
    [Route("api/ecb")]
    public class EcbController : ControllerBase
    {
         private readonly IEcbService _ecbService;
        public EcbController(IEcbService ecbService)
        {
            _ecbService = ecbService;
        }
        
        [HttpGet("rates")]
        public async Task<IActionResult> GetRates()
        {
            var rates = await _ecbService.GetLatestRatesAsync();
            return Ok(rates);
        }
    }
}