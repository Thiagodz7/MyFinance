using MediatR;

namespace MyFinance.Application.Commands
{
    public class DeletarLancamentoCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public DeletarLancamentoCommand(Guid id)
        {
            Id = id;
        }
    }
}
