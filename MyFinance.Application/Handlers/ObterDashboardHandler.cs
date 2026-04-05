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
        private readonly IContaRepository _contaRepo; // [NOVO] Injetado para pegar o saldo atual real

        public ObterDashboardHandler(ILancamentoRepository lancamentoRepo, IContaRepository contaRepo)
        {
            _lancamentoRepo = lancamentoRepo;
            _contaRepo = contaRepo;
        }

        public async Task<DashboardDto> Handle(ObterDashboardQuery request, CancellationToken cancellationToken)
        {
            var todosLancamentos = await _lancamentoRepo.GetByContaIdAsync(request.ContaId);
            var conta = await _contaRepo.GetByIdAsync(request.ContaId);

            decimal saldoDeHoje = conta?.SaldoAtual ?? 0;

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
            // [NOVO] MOTOR DE PREVISIBILIDADE (Próximos 6 meses)
            // =======================================================
            var previsoes = new List<DashboardPrevisaoDto>();
            decimal saldoAcumulado = saldoDeHoje;

            // Adiciona o mês atual como ponto 0 no gráfico
            previsoes.Add(new DashboardPrevisaoDto
            {
                Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(mesAtual).ToUpper(),
                SaldoPrevisto = saldoAcumulado
            });

            // Projeta os próximos 5 meses
            for (int i = 1; i <= 5; i++)
            {
                var dataAlvo = dataAtual.AddMonths(i);

                // 1. Pegamos todos os lançamentos do mês alvo de uma vez para não repetir consultas
                var lancamentosMesAlvo = todosLancamentos
                    .Where(l => l.DataVencimento.Month == dataAlvo.Month && l.DataVencimento.Year == dataAlvo.Year)
                    .ToList();

                // 2. Calculamos o que entra e o que sai especificamente naquele mês
                var receitasMes = lancamentosMesAlvo.Where(l => l.Valor > 0).Sum(l => l.Valor);
                var despesasMes = lancamentosMesAlvo.Where(l => l.Valor < 0).Sum(l => l.Valor);

                // 3. O saldo acumulado continua somando o valor real (positivo + negativo)
                saldoAcumulado += (receitasMes + despesasMes);

                previsoes.Add(new DashboardPrevisaoDto
                {
                    Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dataAlvo.Month).ToUpper(),

                    // Enviamos os valores mensais para as barras Verde e Vermelha
                    Receitas = receitasMes,

                    // Para o gráfico de barras, a despesa deve ser enviada como valor positivo 
                    // para que a barra suba no eixo Y em vermelho. Math.Abs resolve isso.
                    Despesas = Math.Abs(despesasMes),

                    // O saldo projetado acumulado para a barra Azul
                    SaldoPrevisto = saldoAcumulado
                });
            }

            return new DashboardDto
            {
                TotalReceitas = receitas,
                TotalDespesas = Math.Abs(despesas),
                SaldoTotal = receitas + despesas,
                DespesasPorCategoria = porCategoria,
                PrevisaoProximosMeses = previsoes // <--- Alimentando o gráfico novo!
            };
        }
    }
}