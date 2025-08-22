using MediatR;
using WalletApp.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace WalletApp.Application.Commands.Wallets
{
    public class CreateWalletCommand : IRequest<long> //it returns the id of the wallet
    {
        public string Currency { get; set; } //required for objects initializer (walletcontroler)

        public CreateWalletCommand(string currency)
        {
            Currency = currency;
        }
    }
}