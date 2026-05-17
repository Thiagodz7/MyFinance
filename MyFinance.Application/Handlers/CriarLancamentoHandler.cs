using MassTransit;
using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Events;
using MyFinance.Domain.Interfaces;
using static MyFinance.Domain.Entities.Lancamento;

namespace MyFinance.Application.Handlers
{
    public class CriarLancamentoHandler : IRequestHandler<CriarLancamentoCommand, Guid>
    {
        private readonly ILancamentoRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _uow;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IContaRepository _contaRepository;

        public CriarLancamentoHandler(
            ILancamentoRepository repository,
            IPublishEndpoint publishEndpoint,
            IUnitOfWork uow,
            ICategoriaRepository categoriaRepository,
            IContaRepository contaRepository)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
            _uow = uow;
            _categoriaRepository = categoriaRepository;
            _contaRepository = contaRepository;
        }

        public async Task<Guid> Handle(CriarLancamentoCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(request.CategoriaId);
            if (categoria == null)
                throw new Exception("Categoria não encontrada.");

            var conta = await _contaRepository.GetByIdAsync(request.ContaId);
            if (conta == null)
                throw new Exception("Conta bancária não encontrada.");

            conta.AtualizarSaldo(request.Valor);
            await _contaRepository.UpdateAsync(conta.Id, conta, cancellationToken);

            var lancamentosParaSalvar = new List<Lancamento>();
            Guid? grupoId = request.EhRecorrente ? Guid.NewGuid() : null;
            int quantidadeParcelas = request.EhRecorrente ? request.TotalParcelas : 1;

            for (int i = 0; i < quantidadeParcelas; i++)
            {
                DateTime dataParcela = CalcularDataVencimento(request.DataVencimento, request.Frequencia, i);

                var lancamento = new Lancamento(request.Descricao, request.Valor, dataParcela, request.ContaId, request.CategoriaId, request.Pago);

                if (request.EhRecorrente)
                {
                    lancamento.ConfigurarRecorrencia(request.Frequencia, i + 1, quantidadeParcelas, grupoId.Value);
                }

                lancamentosParaSalvar.Add(lancamento);
            }

            await _repository.AddRangeAsync(lancamentosParaSalvar);

            await _uow.CommitAsync();

            var primeiroLancamento = lancamentosParaSalvar.First();

            await _publishEndpoint.Publish(new LancamentoCriadoEvent
            {
                Id = primeiroLancamento.Id,
                Descricao = primeiroLancamento.Descricao,
                Valor = primeiroLancamento.Valor,
                DataOcorrencia = primeiroLancamento.DataVencimento,
                ContaId = request.ContaId,
                CategoriaId = request.CategoriaId
            }, cancellationToken);

            return primeiroLancamento.Id;
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