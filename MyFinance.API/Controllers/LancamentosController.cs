using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Commands;

namespace MyFinance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LancamentosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LancamentosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarLancamentoCommand command)
        {
            // O Controller não sabe logica nenhuma. Ele só diz pro Mediator: "Envia isso!"
            var id = await _mediator.Send(command);

            return Ok(new { Id = id });
        }
    }
}