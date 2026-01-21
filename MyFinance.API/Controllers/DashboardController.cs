using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Application.Queries;

namespace MyFinance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{contaId}")]
        public async Task<IActionResult> ObterDashboard(Guid contaId)
        {
            var result = await _mediator.Send(new ObterDashboardQuery(contaId));
            return Ok(result);
        }
    }
}