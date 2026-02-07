using MediatR;
using MyFinance.Application.DTOs;

namespace MyFinance.Application.Queries
{
    public record ObterLancamentoPorIdQuery(Guid Id) : IRequest<LancamentoDto?>
    {
    }
}
