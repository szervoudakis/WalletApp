using MediatR;
using Novibet.Domain.Entities;

namespace Novibet.Application.Querries.Wallets
{
    public record RetrieveWalletBalanceQuery(long Id) : IRequest<Wallet?>;
}