using MassTransit;
using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Events;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    // IRequestHandler<Entrada, Saida>
    public class CriarLancamentoHandler : IRequestHandler<CriarLancamentoCommand, Guid>
    {
        private readonly ILancamentoRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _uow;
        private readonly ICategoriaRepository _categoriaRepository;

        // Injetamos o Repositório aqui. O Handler não conhece o DbContext, só o contrato!
        public CriarLancamentoHandler(ILancamentoRepository repository, IPublishEndpoint publishEndpoint, IUnitOfWork uow, ICategoriaRepository categoriaRepository)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
            _uow = uow;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<Guid> Handle(CriarLancamentoCommand request, CancellationToken cancellationToken)
        {
            // 1.1 Validação de Regra de Negócio (Fail Fast)
            var categoria = await _categoriaRepository.GetByIdAsync(request.CategoriaId);
            if (categoria == null)
            {
                throw new Exception("Categoria não encontrada.");
                // Num cenário real, usaríamos um Result Pattern ou Notification Pattern pra não soltar Exception
            }

            // 1.1 Salva no Banco (Síncrono)
            // 1.2 Converte o Command (DTO) para a Entidade de Domínio
            var lancamento = new Lancamento(request.Descricao, request.Valor, request.DataVencimento, request.ContaId, request.CategoriaId);

            // 1.3 Chama a Infra para salvar
            await _repository.AddAsync(lancamento);

            // 2.1 Unit of Work efetiva a gravação no banco
            await _uow.CommitAsync();

            // 2.2 Avisa o RabbitMQ (Assíncrono / Fire-and-Forget)
            // Note que não esperamos resposta, apenas jogamos na fila.
            await _publishEndpoint.Publish(new LancamentoCriadoEvent
            {
                Id = lancamento.Id,
                Descricao = lancamento.Descricao,
                Valor = lancamento.Valor,
                DataOcorrencia = lancamento.DataVencimento,
                ContaId = request.ContaId,
                CategoriaId = request.CategoriaId
            }, cancellationToken);


            // 3.0 Retorna o ID gerado
            return lancamento.Id;
        }
    }
}