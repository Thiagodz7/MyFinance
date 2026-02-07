using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class AlterarLancamentoHandler : IRequestHandler<AlterarLancamentoCommand, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILancamentoRepository _repository;
        private readonly IContaRepository _repositoryConta;
        public AlterarLancamentoHandler(IUnitOfWork uow, ILancamentoRepository repository, IContaRepository repositoryConta)
        {
            _uow = uow;
            _repository = repository;
            _repositoryConta = repositoryConta;
        }

        public async Task<Unit> Handle(AlterarLancamentoCommand request, CancellationToken cancellationToken)
        {
            var lancamento = await _repository.GetByIdAsync(request.Id);

            if (lancamento == null)
                throw new Exception("Lançamento não encontrado.");

            var valorAntigo = lancamento.Valor;

            lancamento.Atualizar(request.Descricao, request.Valor, request.DataVencimento);

            var conta = await _repositoryConta.GetByIdAsync(lancamento.ContaId);

            if (conta == null)
            {
                throw new Exception("Conta não encontrada.");
            }

            conta.AtualizarSaldo(-valorAntigo);
            conta.AtualizarSaldo(lancamento.Valor);


            _repository.Update(lancamento);
            await _repositoryConta.UpdateAsync(conta.Id, conta, cancellationToken);

            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}
