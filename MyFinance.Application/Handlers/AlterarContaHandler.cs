using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class AlterarContaHandler : IRequestHandler<AlterarContaCommand, Unit>
    {
        private readonly IContaRepository _repository;
        private readonly IUnitOfWork _uow;

        public AlterarContaHandler(IContaRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Unit> Handle(AlterarContaCommand request, CancellationToken cancellationToken)
        {
            var conta = await _repository.GetByIdAsync(request.Id);

            if (conta == null)
            {
                throw new Exception("Conta não encontrada");
            }

            conta.Atualizar(request.Nome, request.Banco);

            await _repository.UpdateAsync(request.Id, conta, cancellationToken);
            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}
