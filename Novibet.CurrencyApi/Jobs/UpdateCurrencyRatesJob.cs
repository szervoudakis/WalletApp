using System;
using System.Threading.Tasks;
using Novibet.Infrastructure.Repositories;
using Novibet.Domain.Entities;
using Novibet.EcbGateway.Services;
using Novibet.Infrastructure.Services;
using Novibet.Application.Interfaces;

namespace Novibet.CurrencyApi.Jobs
{
    public class UpdateCurrencyRatesJob
    {
        private readonly IEcbGatewayService _ecbgatewayService;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyService _currencyService;

        public UpdateCurrencyRatesJob(IEcbGatewayService ecbgatewayService, ICurrencyRepository currencyRepository, ICurrencyService currencyService)
        {
            _ecbgatewayService = ecbgatewayService;
            _currencyRepository = currencyRepository;
            _currencyService = currencyService;
        }

        public async Task Execute()
        {
            //var rates = await _ecbgatewayService.GetLatestRatesAsync();
            await _currencyService.UpdateCurrencyRatesAsync(); //call currencyservice for update db
        }
    }
}