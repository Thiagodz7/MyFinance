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

            // 1. Filtro do Mês Vigente
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
            // [NOVO] CÁLCULO DO LUCRO ANUAL
            // =======================================================
            var lucroAno = todosLancamentos
                .Where(l => l.DataVencimento.Year == anoAtual)
                .Sum(l => l.Valor);

            // =======================================================
            // MOTOR DE PREVISIBILIDADE
            // =======================================================
            var previsoes = new List<DashboardPrevisaoDto>();

            var fimMesAtual = new DateTime(anoAtual, mesAtual, DateTime.DaysInMonth(anoAtual, mesAtual), 23, 59, 59);
            decimal saldoAcumuladoReal = todosLancamentos.Where(l => l.DataVencimento <= fimMesAtual).Sum(l => l.Valor);

            previsoes.Add(new DashboardPrevisaoDto
            {
                Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(mesAtual).ToUpper(),
                SaldoPrevisto = saldoAcumuladoReal
            });

            for (int i = 1; i <= 5; i++)
            {
                var dataAlvo = dataAtual.AddMonths(i);

                var lancamentosMesAlvo = todosLancamentos
                    .Where(l => l.DataVencimento.Month == dataAlvo.Month && l.DataVencimento.Year == dataAlvo.Year)
                    .ToList();

                var receitasMes = lancamentosMesAlvo.Where(l => l.Valor > 0).Sum(l => l.Valor);
                var despesasMes = lancamentosMesAlvo.Where(l => l.Valor < 0).Sum(l => l.Valor);

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
                SaldoTotal = receitas + despesas,
                LucroPrevistoAno = lucroAno, // <--- Adicionado aqui!
                DespesasPorCategoria = porCategoria,
                PrevisaoProximosMeses = previsoes
            };
        }
    }
}