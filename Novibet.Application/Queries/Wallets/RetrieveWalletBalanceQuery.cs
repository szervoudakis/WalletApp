using MediatR;
using Novibet.Domain.Entities;
using Novibet.Application.DTOs;
namespace Novibet.Application.Querries.Wallets
{
    public record RetrieveWalletBalanceQuery(long Id, string? Currency) : IRequest<WalletBalanceDto?>;
}