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
            // 1.1 Validação de Regra de Negócio (Fail Fast)
            var categoria = await _categoriaRepository.GetByIdAsync(request.CategoriaId);
            if (categoria == null)
                throw new Exception("Categoria não encontrada.");

            var conta = await _contaRepository.GetByIdAsync(request.ContaId);
            if (conta == null)
                throw new Exception("Conta bancária não encontrada.");

            // Mantive a sua lógica original de saldo. (Nota: Isso vai atualizar o saldo apenas 1 vez, com o valor da primeira parcela, o que é ideal para o cenário atual).
            conta.AtualizarSaldo(request.Valor);
            await _contaRepository.UpdateAsync(conta.Id, conta, cancellationToken);

            // =======================================================
            // 1.2 MOTOR DE RECORRÊNCIA
            // =======================================================
            var lancamentosParaSalvar = new List<Lancamento>();
            Guid? grupoId = request.EhRecorrente ? Guid.NewGuid() : null;
            int quantidadeParcelas = request.EhRecorrente ? request.TotalParcelas : 1;

            for (int i = 0; i < quantidadeParcelas; i++)
            {
                // Calcula a data da parcela (Mês 0, Mês 1, Mês 2...)
                DateTime dataParcela = CalcularDataVencimento(request.DataVencimento, request.Frequencia, i);

                // Instancia a Entidade
                var lancamento = new Lancamento(request.Descricao, request.Valor, dataParcela, request.ContaId, request.CategoriaId);

                // Se for recorrente, aplica as configurações extras
                if (request.EhRecorrente)
                {
                    lancamento.ConfigurarRecorrencia(request.Frequencia, i + 1, quantidadeParcelas, grupoId.Value);
                }

                lancamentosParaSalvar.Add(lancamento);
            }

            // 1.3 Chama a Infra para salvar a lista inteira (Bulk Insert)
            await _repository.AddRangeAsync(lancamentosParaSalvar);

            // 2.1 Unit of Work efetiva a gravação no banco
            await _uow.CommitAsync();

            // 2.2 Avisa o RabbitMQ (Assíncrono / Fire-and-Forget)
            // Pegamos o ID da primeira parcela (ou do lançamento único) para disparar no evento
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

            // 3.0 Retorna o ID gerado (da primeira parcela)
            return primeiroLancamento.Id;
        }

        // --- Método Auxiliar para calcular os "pulos" de calendário ---
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