using MediatR;
using MyFinance.Domain.Entities;

namespace MyFinance.Application.Commands
{
    // Retorna Unit (void do MediatR) pois não precisa retornar ID
    public class AtualizarCategoriaCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public TipoCategoria Tipo { get; set; }
    }
}