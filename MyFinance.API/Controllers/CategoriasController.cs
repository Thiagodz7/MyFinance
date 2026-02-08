using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Commands;
using MyFinance.Application.DTOs;
using MyFinance.Application.Queries;

namespace MyFinance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new ObterCategoriasQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CriarCategoriaCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id = id }, command);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AtualizarCategoriaCommand command)
        {
            if (id != command.Id) return BadRequest("IDs não conferem");

            await _mediator.Send(command);
            return NoContent(); 
        }

        [HttpPatch("{id}/status")] 
        public async Task<IActionResult> PatchStatus(Guid id, [FromBody] AlterarStatusDto dto)
        {
            var command = new AlterarStatusCategoriaCommand { Id = id, Ativo = dto.Ativo };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeletarCategoriaCommand(id));
            return NoContent(); 
        }
    }
}