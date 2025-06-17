using MediatR;
using Novibet.Application.Commands.Wallets;
using Novibet.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Novibet.Application.Interfaces;
using Novibet.Application.DTOs;
using System.Linq.Expressions;


namespace Novibet.Application.Handlers.Wallets
{

    public class AdjustWalletBalanceCommandHandler : IRequestHandler<AdjustWalletBalanceCommand, WalletBalanceDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyCacheService _currencyCacheService;

        public AdjustWalletBalanceCommandHandler(IWalletRepository walletRepository, ICurrencyRepository currencyRepository, ICurrencyCacheService currencyCacheService)
        {
            _walletRepository = walletRepository;
            _currencyRepository = currencyRepository;
            _currencyCacheService = currencyCacheService;
        }

        public async Task<WalletBalanceDto> Handle(AdjustWalletBalanceCommand request, CancellationToken cancellationToken)
        {

            if (request.Amount <= 0)
            {
                throw new ArgumentException("Amount must be positive.");
            }

            var wallet = await _walletRepository.RetrieveWalletByIdAsync(request.WalletId);

            if (wallet == null)
            {
                throw new KeyNotFoundException("Wallet not found");
            }
            decimal amountInWalletCurrency = request.Amount;

            if (!string.Equals(wallet.Currency, request.Currency, StringComparison.OrdinalIgnoreCase))
            {
                var rate = await _currencyCacheService.GetCachedRateAsync(request.Currency, wallet.Currency);
                if (rate == null)
                {
                    //try to get rate from db because cache is empty
                    var ratedb = await _currencyRepository.RetrieveCurrencyRateAsync(request.Currency);
                    if (!ratedb.TryGetValue(request.Currency, out var rateFromDb))
                    {
                        throw new Exception($"Currency rate for {request.Currency} not found in database.");
                    }
                    rate = rateFromDb;
                }
                amountInWalletCurrency = request.Amount * rate.Value;
            }

            //switch based on strategy
            switch (request.Strategy)
            {
                case "Increase":
                    wallet.AddFunds(amountInWalletCurrency);
                    break;

                case "Decrease":
                    wallet.SubstractFunds(amountInWalletCurrency);
                    break;
                case "ForceDecrease":
                    wallet.ForceSubtractFunds(amountInWalletCurrency);
                    break;

                default:
                    throw new ArgumentException($"Strategy '{request.Strategy}' is not supported.");
            }

            await _walletRepository.UpdateAsync(wallet);
            return new WalletBalanceDto
            {
                WalletId = wallet.Id,
                Balance = wallet.Balance,
                Currency = wallet.Currency
            };


        }

    }
}    