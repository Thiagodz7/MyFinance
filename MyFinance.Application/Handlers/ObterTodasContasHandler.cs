using MediatR;
using MyFinance.Application.DTOs;
using MyFinance.Application.Queries;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class ObterTodasContasHandler : IRequestHandler<ObterTodasContasQuery, IEnumerable<ContaDto>>
    {
        private readonly IContaRepository _repository;
        private readonly ILancamentoRepository _lancamentoRepo; // [NOVO] Injetado para calcular o saldo real

        public ObterTodasContasHandler(IContaRepository repository, ILancamentoRepository lancamentoRepo)
        {
            _repository = repository;
            _lancamentoRepo = lancamentoRepo;
        }

        public async Task<IEnumerable<ContaDto>> Handle(ObterTodasContasQuery request, CancellationToken cancellationToken)
        {
            var contas = await _repository.GetAllAsync();
            var listaContasDto = new List<ContaDto>();

            var dataAtual = DateTime.Now;
            // Define o teto: Último dia do mês atual às 23:59:59
            var fimMesAtual = new DateTime(dataAtual.Year, dataAtual.Month, DateTime.DaysInMonth(dataAtual.Year, dataAtual.Month), 23, 59, 59);

            foreach (var c in contas)
            {
                // Busca o histórico da conta
                var lancamentos = await _lancamentoRepo.GetByContaIdAsync(c.Id);

                // Calcula o saldo ignorando lançamentos de meses futuros
                var saldoReal = lancamentos
                    .Where(l => l.DataVencimento <= fimMesAtual)
                    .Sum(l => l.Valor);

                listaContasDto.Add(new ContaDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Banco = c.Banco,
                    SaldoAtual = saldoReal, // <-- Substituímos o saldo do banco pelo saldo calculado!
                    Ativo = c.Ativo
                });
            }

            var resultadoFinal = request.ApenasAtivos
                ? listaContasDto.Where(c => c.Ativo).ToList()
                : listaContasDto;

            return resultadoFinal;
        }
    }
}