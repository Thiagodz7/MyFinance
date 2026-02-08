using MediatR;
using MyFinance.Application.Commands;
using MyFinance.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Application.Handlers
{
    public class AlterarStatusContaHandler : IRequestHandler<AlterarStatusContaCommand, Unit>
    {
        private readonly IContaRepository _repository;
        private readonly IUnitOfWork _uow;

        public AlterarStatusContaHandler(IContaRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<Unit> Handle(AlterarStatusContaCommand request, CancellationToken cancellationToken)
        {
            var conta = await _repository.GetByIdAsync(request.Id);

            if (conta == null)
            {
                throw new Exception("Conta não encontrada.");
            }

            conta.AlterarStatus(request.Ativo);

            await _repository.UpdateAsync(conta.Id, conta, cancellationToken);
            await _uow.CommitAsync();

            return Unit.Value;
        }
    }
}
