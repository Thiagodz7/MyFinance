using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class AlterarStatusLancamentoHandler : IRequestHandler<AlterarStatusLancamentoCommand, Unit>
    {
        private readonly ILancamentoRepository _repository;
        private readonly IUnitOfWork _uow;

        public AlterarStatusLancamentoHandler(ILancamentoRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Unit> Handle(AlterarStatusLancamentoCommand request, CancellationToken cancellationToken)
        {
            var lancamento = await _repository.GetByIdAsync(request.Id);
            if (lancamento == null) throw new Exception("Lançamento não encontrado.");

            lancamento.AlterarStatusPagamento(request.Pago);

            _repository.Update(lancamento);
            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}