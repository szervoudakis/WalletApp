using Microsoft.AspNetCore.Mvc;
using Novibet.EcbGateway.Services;


namespace Novibet.CurrencyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly CurrencyCacheService _cacheService;

        public RedisController(CurrencyCacheService cacheService)
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
