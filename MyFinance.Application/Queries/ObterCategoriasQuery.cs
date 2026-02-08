using MediatR;
using MyFinance.Application.DTOs;

namespace MyFinance.Application.Queries
{
    public class ObterCategoriasQuery : IRequest<IEnumerable<CategoriaDto>>
    {
        public bool ApenasAtivos { get; set; } = false; // Padrão traz tudo

        public ObterCategoriasQuery(bool apenasAtivos = false)
        {
            ApenasAtivos = apenasAtivos;
        }
    }
}