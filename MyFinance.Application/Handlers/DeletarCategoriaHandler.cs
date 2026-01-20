using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class DeletarCategoriaHandler : IRequestHandler<DeletarCategoriaCommand, Unit>
    {
        private readonly ICategoriaRepository _repository;
        private readonly IUnitOfWork _uow;

        public DeletarCategoriaHandler(ICategoriaRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Unit> Handle(DeletarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _repository.GetByIdAsync(request.Id);

            if (categoria == null)
            {
                throw new Exception("Categoria não encontrada.");
            }

            // O EF Core vai tentar deletar. Se tiver lançamentos vinculados, 
            // vai estourar erro no CommitAsync (SQL Exception).
            // Num sistema real, trataríamos com try/catch para mensagem amigável.
            _repository.Delete(categoria);
            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}