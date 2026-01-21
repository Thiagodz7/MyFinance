using MediatR;
using MyFinance.Application.DTOs;
using MyFinance.Application.Queries;
using MyFinance.Domain.Interfaces;

namespace MyFinance.Application.Handlers
{
    public class ObterDashboardHandler : IRequestHandler<ObterDashboardQuery, DashboardDto>
    {
        private readonly ILancamentoRepository _lancamentoRepo;

        public ObterDashboardHandler(ILancamentoRepository lancamentoRepo)
        {
            _lancamentoRepo = lancamentoRepo;
        }

        public async Task<DashboardDto> Handle(ObterDashboardQuery request, CancellationToken cancellationToken)
        {
            // 1. Busca TUDO da conta (Num app real, filtraríamos por data no SQL pra não pesar)
            var todosLancamentos = await _lancamentoRepo.GetByContaIdAsync(request.ContaId);

            // 2. Filtra apenas o mês atual para o dashboard (Regra de Negócio)
            var mesAtual = DateTime.Now.Month;
            var anoAtual = DateTime.Now.Year;

            var lancamentosMes = todosLancamentos
                .Where(l => l.DataVencimento.Month == mesAtual && l.DataVencimento.Year == anoAtual)
                .ToList();

            // 3. Calcula Totais
            var receitas = lancamentosMes.Where(l => l.Valor > 0).Sum(l => l.Valor);
            var despesas = lancamentosMes.Where(l => l.Valor < 0).Sum(l => l.Valor);

            // 4. Agrupa Despesas por Categoria (Para o Gráfico)
            var porCategoria = lancamentosMes
                .Where(l => l.Valor < 0) // Só despesas
                .GroupBy(l => l.Categoria?.Nome ?? "Sem Categoria")
                .Select(g => new DashboardCategoriaDto
                {
                    Categoria = g.Key,
                    Valor = Math.Abs(g.Sum(l => l.Valor)) // Math.Abs pra tirar o sinal negativo
                })
                .OrderByDescending(x => x.Valor) // As maiores primeiro
                .ToList();

            return new DashboardDto
            {
                TotalReceitas = receitas,
                TotalDespesas = Math.Abs(despesas), // Envia positivo pro front exibir bonito
                SaldoTotal = receitas + despesas, // Saldo real (considera o negativo)
                DespesasPorCategoria = porCategoria
            };
        }
    }
}