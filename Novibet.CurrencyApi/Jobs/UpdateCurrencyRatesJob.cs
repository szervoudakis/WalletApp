using System;
using System.Threading.Tasks;
using Novibet.Infrastructure.Repositories;
using Novibet.Domain.Entities;
using Novibet.EcbGateway.Services;
using Novibet.CurrencyApi.Services;

namespace Novibet.CurrencyApi.Jobs
{
    public class UpdateCurrencyRatesJob
    {
        private readonly IEcbService _ecbService;
        private readonly CurrencyRepository _currencyRepository;

        private readonly CurrencyService _currencyService;

        public UpdateCurrencyRatesJob(IEcbService ecbService, CurrencyRepository currencyRepository, CurrencyService currencyService)
        {
            _ecbService = ecbService;
            _currencyRepository = currencyRepository;
            _currencyService = currencyService;
        }

        public async Task Execute()
        {
            //var rates = await _ecbService.GetLatestRatesAsync();
            await _currencyService.UpdateCurrencyRatesAsync(); //call currencyservice for update db
        }
    }
}