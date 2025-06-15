using AutoMapper;
using Novibet.Infrastructure.Repositories;
using Novibet.Domain.Entities;
using Novibet.EcbGateway.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Novibet.CurrencyApi.Services
{
    public class CurrencyService
    {
        private readonly IEcbService _ecbService;
        private readonly CurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        private readonly CurrencyCacheService _currencyCacheService;

        public CurrencyService(IEcbService ecbService, CurrencyRepository currencyRepository, IMapper mapper, CurrencyCacheService currencyCacheService)
        {
            _ecbService = ecbService;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _currencyCacheService = currencyCacheService;
        }

        public async Task UpdateCurrencyRatesAsync()
        {
            var rates = await _ecbService.GetLatestRatesAsync(); //get latest rates
            var currencies = _mapper.Map<List<Currency>>(rates);  //convert to currencies
            // await _currencyRepository.SaveCurrencyRatesAsync(currencies); //call save from currencyRepo for updates
        }

        public async Task<Dictionary<string, decimal>> GetCurrencyRatesAsync()
        {
            var cachedRates = await _currencyCacheService.GetCachedCurrencyRatesAsync();
            if (cachedRates != null && cachedRates.Count > 0)
            {
                Console.WriteLine("exei cached data"); 
                return cachedRates;
            }

            try
            {
                //if rates dont exist in cache we retrieve from db
                var currencies = await _currencyRepository.GetLatestRatesAsync();
                if (currencies == null || !currencies.Any())
                {
                    throw new Exception("Failed to retrieve currency rates from ECB.");
                }
                //convert rates to dictionary
                var rates = currencies.ToDictionary(c => c.CurrencyCode, c => c.Rate);
                //we store the updated rates in Redis cache for future calls
                await _currencyCacheService.SetCurrencyRatesAsync(rates);
                return rates;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving currency rates: {ex.Message}");
                return new Dictionary<string, decimal>();  //empty dictionary if we have exception 
            }            
            
        }
    }
}
