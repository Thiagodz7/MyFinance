using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Commands;
using MyFinance.Application.Queries;

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new DeletarLancamentoCommand(id));
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AlterarLancamentoCommand command)
        {
            if (id != command.Id) return BadRequest("IDs não conferem");
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var resultado = await _mediator.Send(new ObterLancamentoPorIdQuery(id));

            if (resultado == null) return NotFound();

            return Ok(resultado);
        }
    }
}