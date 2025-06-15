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

        public CreateWalletCommandHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }
        public async Task<long> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = new Wallet(0, request.Currency); // create Wallet
            var walletId = await _walletRepository.CreateAsync(wallet); // store walet info in db

            return walletId;
        }
    }
}