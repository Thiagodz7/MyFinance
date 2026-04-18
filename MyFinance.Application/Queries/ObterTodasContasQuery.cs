using MediatR;
using MyFinance.Shared.DTOs;

namespace MyFinance.Application.Queries
{
    public class ObterTodasContasQuery : IRequest<IEnumerable<ContaDto>>
    {
        public bool ApenasAtivos { get; set; } = false; // Padrão traz tudo

        public ObterTodasContasQuery(bool apenasAtivos = false)
        {
            ApenasAtivos = apenasAtivos;
        }
    }
}