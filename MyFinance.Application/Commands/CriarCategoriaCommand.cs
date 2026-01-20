using MediatR;
using MyFinance.Domain.Entities; // Para usar o Enum TipoCategoria

namespace MyFinance.Application.Commands
{
    // Retorna o Guid da nova categoria
    public class CriarCategoriaCommand : IRequest<Guid>
    {
        public string Nome { get; set; } = string.Empty;
        public TipoCategoria Tipo { get; set; }
    }
}