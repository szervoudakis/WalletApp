using Microsoft.AspNetCore.Mvc;
using WalletApp.EcbGateway.Services;
using WalletApp.Application.Interfaces;

namespace WalletApp.CurrencyApi.Controllers
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
