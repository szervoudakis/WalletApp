using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Application.Commands.Wallets;
using WalletApp.Application.DTOs;
using WalletApp.Application.Querries.Wallets;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WalletApp.CurrencyApi.Controllers
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
        public async Task<IActionResult> GetWalletBalance(long walletId, [FromQuery] string? currency = null)
        {
            var balance = await _mediator.Send(new RetrieveWalletBalanceQuery(walletId, currency));
            return Ok(new { walletId = walletId, Balance = balance });
        }
        [Authorize]
        [HttpPost("{walletId}/adjustbalance")]
        public async Task<ActionResult<WalletBalanceDto>> AdjustBalance([FromRoute] long walletId, [FromQuery] decimal amount, [FromQuery] string currency, [FromQuery] string strategy)
        {
            var command = new AdjustWalletBalanceCommand
            {
                WalletId = walletId,
                Amount = amount,
                Currency = currency,
                Strategy = strategy
            };

            var result = await _mediator.Send(command);
            return Ok(result);
            
        } 
        
    }
}