using MediatR;
using Novibet.Application.Commands.Wallets;
using Novibet.Application.DTOs;
using Novibet.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Novibet.Application.Interfaces;
using Novibet.Application.Querries.Wallets;

namespace Novibet.Application.Handlers.Wallets  {
  public class RetrieveWalletBalanceQueryHandler : IRequestHandler<RetrieveWalletBalanceQuery, WalletBalanceDto?>
  {
    private readonly IWalletRepository _walletRepository;
    private readonly ICurrencyService _currencyService;
    public RetrieveWalletBalanceQueryHandler(IWalletRepository walletRepository, ICurrencyService currencyService)
    {
      _walletRepository = walletRepository;
      _currencyService = currencyService;
    }

    public async Task<WalletBalanceDto?> Handle(RetrieveWalletBalanceQuery request, CancellationToken cancellationToken)
    {
      var wallet = await _walletRepository.RetrieveWalletByIdAsync(request.Id);
      if (wallet == null)
      {
         throw new KeyNotFoundException($"Wallet {request.Id} not found.");
      }
      decimal convertedBalance = wallet.Balance;
      string targetCur = request.Currency ?? wallet.Currency;

      //comparison between two currencies target,and wallet currency
      if (!string.Equals(wallet.Currency, targetCur,StringComparison.OrdinalIgnoreCase))
      {
        // decimal amount, string fromCurrency, string toCurrency
        convertedBalance = await _currencyService.ConvertAsync(wallet.Balance,wallet.Currency, targetCur);
      }
      
      return new WalletBalanceDto
      {
        WalletId = request.Id,
        Balance = convertedBalance,
        Currency = targetCur
      };
    }    
  }    
}
