using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Application.Commands.Conta;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Shared.Enums;

namespace MyFinance.Application.Handlers
{
    public class DuplicarContaSimulacaoHandler : IRequestHandler<DuplicarContaSimulacaoCommand, Guid>
    {
        private readonly IContaRepository _repository;
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUserService;

        // Injetando APENAS as interfaces, sem acoplamento com o Entity Framework
        public DuplicarContaSimulacaoHandler(
            IContaRepository repository,
            IUnitOfWork uow,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _uow = uow;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(DuplicarContaSimulacaoCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var contaOriginal = await _repository.GetByIdAsyncForDuplicate(request.ContaOriginalId, userId);
            if (contaOriginal == null) throw new Exception("Conta original não encontrada.");

            var novaConta = new Conta(
                nome: request.NomeSimulacao,
                saldoInicial: contaOriginal.SaldoAtual,
                banco: contaOriginal.Banco,
                tipo: TipoConta.Simulacao);

            //novaConta.MarcarComoSimulacao(); 
            novaConta.AssociarUsuario(userId);

            await _repository.AddAsync(novaConta, cancellationToken);
            await _uow.CommitAsync();

            await _repository.ClonarLancamentosAsync(request.ContaOriginalId, novaConta.Id, userId);

            return novaConta.Id;
        }
    }
}