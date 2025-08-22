using MediatR;
using WalletApp.Domain.Entities;
using WalletApp.Application.DTOs;
namespace WalletApp.Application.Querries.Wallets
{
    public record RetrieveWalletBalanceQuery(long Id, string? Currency) : IRequest<WalletBalanceDto?>;
}