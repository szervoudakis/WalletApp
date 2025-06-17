using AutoMapper;
using Novibet.Infrastructure.Repositories;
using Novibet.Domain.Entities;
using Novibet.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Novibet.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IEcbService _ecbService;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        private readonly ICurrencyCacheService _currencyCacheService;

        public CurrencyService(IEcbService ecbService, ICurrencyRepository currencyRepository, IMapper mapper, ICurrencyCacheService currencyCacheService)
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
            await _currencyRepository.SaveCurrencyRatesAsync(currencies); //call save from currencyRepo for updates

            try
            {//write-through cache update
                var ratesDict = currencies.ToDictionary(c => c.CurrencyCode, c => c.Rate);
                await _currencyCacheService.SetCurrencyRatesAsync(ratesDict);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cache update failed:{ex.Message}");
            }
        }

        public async Task<Dictionary<string, decimal>> GetCurrencyRatesAsync()
        {
            var cachedRates = await _currencyCacheService.GetCachedCurrencyRatesAsync();
            if (cachedRates != null && cachedRates.Count > 0)
            {
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

        public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
        {
            var rates = await _currencyCacheService.GetCachedCurrencyRatesAsync();  //get rates from cache
            
            if (rates == null )
            {
                throw new Exception("Currency rate not available for conversion");
            }
            if (!rates.ContainsKey("EUR"))
            {
                rates["EUR"] = 1.0m;
            }

            //todo retrieve from db if cache is empty

            decimal fromRate = rates[fromCurrency];
            decimal toRate = rates[toCurrency];

            if (!rates.ContainsKey(fromCurrency) || !rates.ContainsKey(toCurrency))
            {
                throw new Exception("Currency rate not available for conversion");
            }

            //conversion
            decimal baseAmount = amount / fromRate;
            decimal convertedAmount = baseAmount * toRate;

            return Math.Round(convertedAmount, 2);
        }
    }
}
