using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Entities;
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

            if (!lancamento.EhRecorrente && request.EhRecorrente && request.TotalParcelas > 1)
            {
                var grupoId = Guid.NewGuid();

                lancamento.ConfigurarRecorrencia(request.Frequencia, 1, request.TotalParcelas, grupoId);

                var novosLancamentosFuturos = new List<Lancamento>();

                for (int i = 1; i < request.TotalParcelas; i++)
                {
                    DateTime dataParcela = CalcularDataVencimento(request.DataVencimento, request.Frequencia, i);

                    var novaParcela = new Lancamento(
                        request.Descricao,
                        request.Valor,
                        dataParcela,
                        lancamento.ContaId,
                        lancamento.CategoriaId,
                        request.Pago);

                    novaParcela.ConfigurarRecorrencia(request.Frequencia, i + 1, request.TotalParcelas, grupoId);

                    novosLancamentosFuturos.Add(novaParcela);
                }

                await _repository.AddRangeAsync(novosLancamentosFuturos);
            }

            _repository.Update(lancamento);
            await _repositoryConta.UpdateAsync(conta.Id, conta, cancellationToken);

            await _uow.CommitAsync();

            return Unit.Value;
        }

        private DateTime CalcularDataVencimento(DateTime dataBase, TipoFrequencia frequencia, int incrementoDeCiclos)
        {
            if (incrementoDeCiclos == 0) return dataBase;

            return frequencia switch
            {
                TipoFrequencia.Semanal => dataBase.AddDays(7 * incrementoDeCiclos),
                TipoFrequencia.Mensal => dataBase.AddMonths(incrementoDeCiclos),
                TipoFrequencia.Anual => dataBase.AddYears(incrementoDeCiclos),
                _ => dataBase
            };
        }
    }
}