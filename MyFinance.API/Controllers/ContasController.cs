using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Commands;
using MyFinance.Application.DTOs;
using MyFinance.Application.Queries;

namespace MyFinance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Contas/{id}/extrato
        [HttpGet("{id}/extrato")]
        public async Task<IActionResult> ObterExtrato(Guid id)
        {
            var query = new ObterExtratoQuery(id);
            var resultado = await _mediator.Send(query);

            return Ok(resultado);
        }

        // GET: api/Contas
        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            var query = new ObterTodasContasQuery();
            var resultado = await _mediator.Send(query);
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarContaCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(new { Id = id });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PatchStatus(Guid id, [FromBody] AlterarStatusDto dto)
        {
            var command = new AlterarStatusContaCommand { Id = id, Ativo = dto.Ativo };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] AlterarContaCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}