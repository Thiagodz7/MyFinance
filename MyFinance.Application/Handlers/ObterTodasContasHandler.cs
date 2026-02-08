using MediatR;
using MyFinance.Application.DTOs;
using MyFinance.Application.Queries;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class ObterTodasContasHandler : IRequestHandler<ObterTodasContasQuery, IEnumerable<ContaDto>>
    {
        private readonly IContaRepository _repository;

        public ObterTodasContasHandler(IContaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContaDto>> Handle(ObterTodasContasQuery request, CancellationToken cancellationToken)
        {
            var contas = await _repository.GetAllAsync();

            var listaContasDto = contas.Select(c => new ContaDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Banco = c.Banco,
                SaldoAtual = c.SaldoAtual,
                Ativo = c.Ativo
            });

            if (request.ApenasAtivos)
            {
                listaContasDto.Where(c => c.Ativo);
            }

            return listaContasDto;
        }
    }
}