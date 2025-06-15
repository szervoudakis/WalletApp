using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Novibet.Application.Commands.Wallets;
using Novibet.Application.Querries.Wallets;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletCommand command)
        {
            var walletId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetWalletBalance), new { walletId }, walletId);
        }

        // retrieve specific wallet balance by id
        [Authorize]
        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetWalletBalance(long walletId)
        {
            var balance = await _mediator.Send(new RetrieveWalletBalanceQuery(walletId));
            if (balance == null)
            {
                return NotFound($"Wallet {walletId} not found.");
            }

            return Ok(new { walletId = walletId, Balance = balance });
        }
        
    }
}