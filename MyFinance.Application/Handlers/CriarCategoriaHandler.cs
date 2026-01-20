using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class CriarCategoriaHandler : IRequestHandler<CriarCategoriaCommand, Guid>
    {
        private readonly ICategoriaRepository _repository;
        private readonly IUnitOfWork _uow;

        public CriarCategoriaHandler(ICategoriaRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Guid> Handle(CriarCategoriaCommand request, CancellationToken cancellationToken)
        {
            // Poderia validar se já existe categoria com mesmo nome aqui...

            var categoria = new Categoria(request.Nome, request.Tipo);

            await _repository.AddAsync(categoria);
            await _uow.CommitAsync();

            return categoria.Id;
        }
    }
}