using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class AlterarStatusCategoriaHandle : IRequestHandler<AlterarStatusCategoriaCommand, Unit>
    {
        private readonly ICategoriaRepository _repository;
        private readonly IUnitOfWork _uow;

        public AlterarStatusCategoriaHandle(ICategoriaRepository categoriaRepository, IUnitOfWork unitOfWork)
        {
            _repository = categoriaRepository;
            _uow = unitOfWork;
        }

        public async Task<Unit> Handle(AlterarStatusCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _repository.GetByIdAsync(request.Id);

            if (categoria == null)
            {
                throw new Exception("Categoria não encontrada.");
            }

            categoria.AlterarStatus(request.Ativo);

            await _repository.UpdateAsync(categoria.Id, categoria, cancellationToken);
            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}
