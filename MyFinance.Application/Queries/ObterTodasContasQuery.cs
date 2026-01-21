using MediatR;
using MyFinance.Application.DTOs;

namespace MyFinance.Application.Queries
{
    public class ObterTodasContasQuery : IRequest<IEnumerable<ContaDto>>
    {
    }
}