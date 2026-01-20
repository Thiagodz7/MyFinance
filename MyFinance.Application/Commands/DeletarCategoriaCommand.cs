using MediatR;

namespace MyFinance.Application.Commands
{
    public class DeletarCategoriaCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public DeletarCategoriaCommand(Guid id)
        {
            Id = id;
        }
    }
}