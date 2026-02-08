using MediatR;

namespace MyFinance.Application.Commands
{
    public class AlterarStatusContaCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
    }
}
