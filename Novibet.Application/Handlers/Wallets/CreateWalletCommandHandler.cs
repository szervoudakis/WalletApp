using MediatR;
using Novibet.Application.Commands.Wallets;
using Novibet.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Novibet.Application.Interfaces;

namespace Novibet.Application.Handlers.Wallets
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, long>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ICurrencyCacheService _currencyCacheService;
        private readonly ICurrencyRepository _currencyRepository;

        public CreateWalletCommandHandler(IWalletRepository walletRepository,ICurrencyCacheService currencyCacheService, ICurrencyRepository currencyRepository)
        {
            _walletRepository = walletRepository;
            _currencyCacheService=currencyCacheService;
            _currencyRepository=currencyRepository;
        }

        public async Task<long> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var cachedRates = await _currencyCacheService.GetCachedCurrencyRatesAsync();
            if (!cachedRates.ContainsKey(request.Currency))
            {
                //if requested rate dont exists in cache we checking if exists in db
                var dbRates = await _currencyRepository.RetrieveCurrencyRateAsync(request.Currency);
                if (!dbRates.ContainsKey(request.Currency)){
                    throw new ArgumentException($"Invalid currency: {request.Currency}");
                }
                 
            }
            var wallet = new Wallet(0, request.Currency); // create Wallet
            var walletId = await _walletRepository.CreateAsync(wallet); // store walet info in db

            return walletId;
        }
    }
}