using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Novibet.EcbGateway.Services;
using Novibet.Domain.Entities;
using Novibet.Infrastructure.Repositories;

namespace Novibet.CurrencyApi.Controllers
{
    [ApiController]
    [Route("api/ecb")]
    public class EcbController : ControllerBase
    {
        private readonly IEcbService _ecbService;
        private readonly IMapper _mapper;

        private readonly CurrencyRepository _currencyRepository;
        public EcbController(IEcbService ecbService, IMapper mapper, CurrencyRepository currencyRepository)
        {
            _mapper = mapper;
            _ecbService = ecbService;
            _currencyRepository = currencyRepository;
        }
        
        [HttpGet("rates")]
        public async Task<IActionResult> GetRates()
        {
            var rates = await _ecbService.GetLatestRatesAsync();
            var currencies = _mapper.Map<List<Currency>>(rates);

            await _currencyRepository.SaveCurrencyRatesAsync(currencies);
            
            return Ok(currencies);
        }
    }
}