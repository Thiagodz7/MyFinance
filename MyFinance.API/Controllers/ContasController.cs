using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    }
}