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

        // Injetamos o Repositório aqui. O Handler não conhece o DbContext, só o contrato!
        public CriarLancamentoHandler(ILancamentoRepository repository, IPublishEndpoint publishEndpoint, IUnitOfWork uow)
        {
            _repository = repository;
            _publishEndpoint = publishEndpoint;
            _uow = uow;
        }

        public async Task<Guid> Handle(CriarLancamentoCommand request, CancellationToken cancellationToken)
        {
            // 1.0 Salva no Banco (Síncrono)
            // 1.1 Converte o Command (DTO) para a Entidade de Domínio
            var lancamento = new Lancamento(request.Descricao, request.Valor, request.DataVencimento, request.ContaId);

            // 1.2 Chama a Infra para salvar
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
                ContaId = request.ContaId
            }, cancellationToken);


            // 3.0 Retorna o ID gerado
            return lancamento.Id;


        }
    }
}