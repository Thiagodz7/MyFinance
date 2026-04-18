using MediatR;
using MyFinance.Shared.Enums;

namespace MyFinance.Application.Commands
{
    public class AlterarContaCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;
        public TipoConta Tipo { get; set; } = TipoConta.Corrente;
    }
}
