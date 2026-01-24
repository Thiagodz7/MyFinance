using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces; // Certifique-se que a Entidade Conta está acessível

namespace MyFinance.Application.Handlers
{
    public class CriarContaHandler : IRequestHandler<CriarContaCommand, Guid>
    {
        private readonly IContaRepository _repository;
        private readonly IUnitOfWork _uow;
        public CriarContaHandler(IContaRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Guid> Handle(CriarContaCommand request, CancellationToken cancellationToken)
        {
            // 1. Criar a Entidade (Domínio)
            var novaConta = new Conta(request.Nome, request.SaldoInicial, request.Banco);

            // 2. Adicionar ao Banco
            await _repository.AddAsync(novaConta, cancellationToken);
            await _uow.CommitAsync();

            // 3. Retornar o ID gerado
            return novaConta.Id;
        }
    }
}