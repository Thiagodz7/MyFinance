using MediatR;

namespace MyFinance.Application.Commands
{
    public class AlterarStatusLancamentoCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public bool Pago { get; set; }
    }
}