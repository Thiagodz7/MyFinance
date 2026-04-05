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
            // 1. Busca o lançamento alvo (IMPORTANTE: Sem AsNoTracking para o EF poder deletar)
            var lancamento = await _repository.GetByIdAsync(request.Id);
            if (lancamento == null) throw new Exception("Lançamento não encontrado.");

            var conta = await _contaRepository.GetByIdAsync(lancamento.ContaId);
            if (conta == null) throw new Exception("Conta não encontrada.");

            var lancamentosParaDeletar = new List<Lancamento>();

            if (request.TipoExclusao == TipoExclusaoRecorrencia.ApenasEste || !lancamento.GrupoRecorrenciaId.HasValue)
            {
                lancamentosParaDeletar.Add(lancamento);
            }
            else
            {
                // 2. Busca todos os irmãos do grupo
                var irmaos = await _repository.ObterPorGrupoIdAsync(lancamento.GrupoRecorrenciaId.Value);

                if (request.TipoExclusao == TipoExclusaoRecorrencia.EsteEProximos)
                {
                    // CORREÇÃO: Comparamos apenas a DATA, ignorando horas que podem causar falha no filtro >=
                    // Além disso, garantimos que a lista não esteja vazia
                    lancamentosParaDeletar = irmaos
                        .Where(x => x.DataVencimento.Date >= lancamento.DataVencimento.Date)
                        .ToList();
                }
                else if (request.TipoExclusao == TipoExclusaoRecorrencia.TodosDoGrupo)
                {
                    lancamentosParaDeletar = irmaos.ToList();
                }

                // SEGURANÇA: Se por algum motivo o principal não estiver na lista de irmãos filtrada, adicionamos ele
                if (!lancamentosParaDeletar.Any(x => x.Id == lancamento.Id))
                {
                    lancamentosParaDeletar.Add(lancamento);
                }
            }

            // 3. Soma o valor real e reverte o saldo
            var valorTotalReverter = lancamentosParaDeletar.Sum(x => x.Valor);
            conta.AtualizarSaldo(-valorTotalReverter);

            await _contaRepository.UpdateAsync(conta.Id, conta, cancellationToken);

            // 4. Executa a deleção em lote
            _repository.DeletarVarios(lancamentosParaDeletar);

            await _uow.CommitAsync();
            return Unit.Value;
        }
    }
}