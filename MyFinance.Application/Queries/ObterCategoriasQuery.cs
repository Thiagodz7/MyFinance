using MediatR;
using MyFinance.Application.DTOs;

namespace MyFinance.Application.Queries
{
    public class ObterCategoriasQuery : IRequest<IEnumerable<CategoriaDto>>
    {
    }
}