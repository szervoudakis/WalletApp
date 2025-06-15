using MediatR;
using Novibet.Application.Commands.Wallets;
using Novibet.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Novibet.Application.Interfaces;
using Novibet.Application.Querries.Wallets;

namespace Novibet.Application.Handlers.Wallets  {
  public class RetrieveWalletBalanceQueryHandler : IRequestHandler<RetrieveWalletBalanceQuery, Wallet?>
  {
       private readonly IWalletRepository _walletRepository;

        public RetrieveWalletBalanceQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Wallet?> Handle(RetrieveWalletBalanceQuery request, CancellationToken cancellationToken)
        {
            return await _walletRepository.RetrieveWalletByIdAsync(request.Id);
        }    
  }    
}
