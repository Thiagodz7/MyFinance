using MediatR;
using MyFinance.Application.DTOs;
using MyFinance.Application.Queries;
using MyFinance.Domain.Interfaces;
using System.Globalization;

namespace MyFinance.Application.Handlers
{
    public class ObterDashboardHandler : IRequestHandler<ObterDashboardQuery, DashboardDto>
    {
        private readonly ILancamentoRepository _lancamentoRepo;
        private readonly IContaRepository _contaRepo;

        public ObterDashboardHandler(ILancamentoRepository lancamentoRepo, IContaRepository contaRepo)
        {
            _lancamentoRepo = lancamentoRepo;
            _contaRepo = contaRepo;
        }

        public async Task<DashboardDto> Handle(ObterDashboardQuery request, CancellationToken cancellationToken)
        {
            var todosLancamentos = await _lancamentoRepo.GetByContaIdAsync(request.ContaId);

            var dataAtual = DateTime.Now;
            var mesAtual = dataAtual.Month;
            var anoAtual = dataAtual.Year;

            // Filtro para os cards superiores (Apenas o mês vigente)
            var lancamentosMes = todosLancamentos
                .Where(l => l.DataVencimento.Month == mesAtual && l.DataVencimento.Year == anoAtual)
                .ToList();

            var receitas = lancamentosMes.Where(l => l.Valor > 0).Sum(l => l.Valor);
            var despesas = lancamentosMes.Where(l => l.Valor < 0).Sum(l => l.Valor);

            var porCategoria = lancamentosMes
                .Where(l => l.Valor < 0)
                .GroupBy(l => l.Categoria?.Nome ?? "Sem Categoria")
                .Select(g => new DashboardCategoriaDto
                {
                    Categoria = g.Key,
                    Valor = Math.Abs(g.Sum(l => l.Valor))
                })
                .OrderByDescending(x => x.Valor)
                .ToList();

            // =======================================================
            // MOTOR DE PREVISIBILIDADE CORRIGIDO
            // =======================================================
            var previsoes = new List<DashboardPrevisaoDto>();

            // O BUG ESTAVA AQUI: conta.SaldoAtual já inclui o futuro.
            // Para projetar, precisamos saber o Saldo Real acumulado APENAS até o fim deste mês.
            var fimMesAtual = new DateTime(anoAtual, mesAtual, DateTime.DaysInMonth(anoAtual, mesAtual), 23, 59, 59);
            decimal saldoAcumuladoReal = todosLancamentos.Where(l => l.DataVencimento <= fimMesAtual).Sum(l => l.Valor);

            // Adiciona o mês atual como ponto 0 no gráfico
            previsoes.Add(new DashboardPrevisaoDto
            {
                Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(mesAtual).ToUpper(),
                SaldoPrevisto = saldoAcumuladoReal
            });

            // Projeta os próximos 5 meses com perfeição
            for (int i = 1; i <= 5; i++)
            {
                var dataAlvo = dataAtual.AddMonths(i);

                var lancamentosMesAlvo = todosLancamentos
                    .Where(l => l.DataVencimento.Month == dataAlvo.Month && l.DataVencimento.Year == dataAlvo.Year)
                    .ToList();

                var receitasMes = lancamentosMesAlvo.Where(l => l.Valor > 0).Sum(l => l.Valor);
                var despesasMes = lancamentosMesAlvo.Where(l => l.Valor < 0).Sum(l => l.Valor);

                // Como partimos de um saldo seguro, agora podemos apenas somar o fluxo do mês alvo
                saldoAcumuladoReal += (receitasMes + despesasMes);

                previsoes.Add(new DashboardPrevisaoDto
                {
                    Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dataAlvo.Month).ToUpper(),
                    Receitas = receitasMes,
                    Despesas = Math.Abs(despesasMes),
                    SaldoPrevisto = saldoAcumuladoReal
                });
            }

            return new DashboardDto
            {
                TotalReceitas = receitas,
                TotalDespesas = Math.Abs(despesas),
                SaldoTotal = receitas + despesas, // Lucro Líquido do Mês Atual
                DespesasPorCategoria = porCategoria,
                PrevisaoProximosMeses = previsoes
            };
        }
    }
}