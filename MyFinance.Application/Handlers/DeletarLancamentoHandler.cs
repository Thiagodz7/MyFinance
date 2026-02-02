using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class DeletarLancamentoHandler : IRequestHandler<DeletarLancamentoCommand, Unit>
    {
        private readonly ILancamentoRepository _repository;
        private readonly IContaRepository _contaRepository;
        private readonly IUnitOfWork _uow;

        public DeletarLancamentoHandler(ILancamentoRepository repository, IUnitOfWork uow, IContaRepository contaRepository)
        {
            _repository = repository;
            _uow = uow;
            _contaRepository = contaRepository;
        }

        public async Task<Unit> Handle(DeletarLancamentoCommand request, CancellationToken cancellationToken)
        {
            // 1. Busca o lançamento para saber o valor e a conta
            var lancamento = await _repository.GetByIdAsync(request.Id);

            if (lancamento == null)
            {
                throw new Exception("Lançamento não encontrado.");
            }

            // 2. Busca a conta vinculada
            var conta = await _contaRepository.GetByIdAsync(lancamento.ContaId);
            
            if (conta == null)
            {
                throw new Exception("Conta não encontrada.");
            }

            // 3. REVERSÃO: Passamos o valor invertido para o método que você já criou
            // Se deletar uma despesa de -50, passamos -(-50) = +50 (devolve o dinheiro)
            conta.AtualizarSaldo(-lancamento.Valor);

            // 4. Persistência
            await _contaRepository.UpdateAsync(conta.Id, conta, cancellationToken);
            _repository.Deletar(lancamento);

            await _uow.CommitAsync();
            return Unit.Value;
        }
    }
}
