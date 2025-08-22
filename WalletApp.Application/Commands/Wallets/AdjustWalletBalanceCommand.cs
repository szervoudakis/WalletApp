using MediatR;
using WalletApp.Application.DTOs;
using WalletApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace WalletApp.Application.Commands.Wallets
{
    public class AdjustWalletBalanceCommand : IRequest<WalletBalanceDto>
    {
        public long WalletId { get; init; }
        public decimal Amount { get; init; }
        public string Currency { get; init; } = default!;
        public string Strategy { get; init; } = default!;
        public AdjustWalletBalanceCommand() {}  //required for objects initializer (walletcontroler)
        public AdjustWalletBalanceCommand(long walletId, decimal amount, string currency, string strategy)
        {
            WalletId = walletId;
            Amount = amount;
            Currency = currency;
            Strategy = strategy;
        }
    }
}