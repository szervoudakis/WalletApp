using Microsoft.AspNetCore.Mvc;
using Novibet.EcbGateway.Services;
using Novibet.Application.Interfaces;

namespace Novibet.CurrencyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly ICurrencyCacheService _cacheService;

        public RedisController(ICurrencyCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("ping")]
        public async Task<IActionResult> PingRedis()
        {
            var result = await _cacheService.PingRedisAsync();
            return Ok(result);
        }
    }
 
}
