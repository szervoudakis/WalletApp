using MediatR;
using WalletApp.Application.Commands.Wallets;
using WalletApp.Application.DTOs;
using WalletApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using WalletApp.Application.Interfaces;
using WalletApp.Application.Querries.Wallets;

namespace WalletApp.Application.Handlers.Wallets  {
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
