using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class AtualizarCategoriaHandler : IRequestHandler<AtualizarCategoriaCommand, Unit>
    {
        private readonly ICategoriaRepository _repository;
        private readonly IUnitOfWork _uow;

        public AtualizarCategoriaHandler(ICategoriaRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Unit> Handle(AtualizarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _repository.GetByIdAsync(request.Id);

            if (categoria == null)
            {
                throw new Exception("Categoria não encontrada.");
            }

            // Atualiza os dados da entidade
            categoria.Atualizar(request.Nome, request.Tipo);

            // Marca como modificado no EF
            _repository.Update(categoria);

            // Persiste no Banco
            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}