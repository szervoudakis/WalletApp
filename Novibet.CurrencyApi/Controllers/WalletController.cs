using MediatR;
using Microsoft.AspNetCore.Mvc;
using Novibet.Application.Commands.Wallets;
using System.Threading.Tasks;

namespace Novibet.CurrencyApi.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletCommand command)
        {
            var walletId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetWalletBalance), new { walletId }, walletId);
        }

        // Placeholder για το GET Wallet Balance (θα το προσθέσουμε στη συνέχεια)
        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetWalletBalance(long walletId)
        {
            return Ok($"Balance for wallet {walletId} (To be implemented)");
        }
    }
}