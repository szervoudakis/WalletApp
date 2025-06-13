using AutoMapper;
using Novibet.Infrastructure.Repositories;
using Novibet.Domain.Entities;
using Novibet.EcbGateway.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Novibet.CurrencyApi.Services
{
    public class CurrencyService
    {
        private readonly IEcbService _ecbService;
        private readonly CurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyService(IEcbService ecbService, CurrencyRepository currencyRepository, IMapper mapper)
        {
            _ecbService = ecbService;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }
        
        public async Task UpdateCurrencyRatesAsync()
        {
            var rates = await _ecbService.GetLatestRatesAsync(); //get latest rates
            var currencies = _mapper.Map<List<Currency>>(rates);  //convert to currencies
            await _currencyRepository.SaveCurrencyRatesAsync(currencies); //call save from currencyRepo for updates
        }
    }
}
