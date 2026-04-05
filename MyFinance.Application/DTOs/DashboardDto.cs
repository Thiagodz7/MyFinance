namespace MyFinance.Application.DTOs
{
    public class DashboardDto
    {
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal SaldoTotal { get; set; }

        // Para o Gráfico de Rosca (Top categorias de despesa do mês atual)
        public List<DashboardCategoriaDto> DespesasPorCategoria { get; set; } = new();

        // [NOVO] Para o Gráfico de Barras (Previsibilidade de Caixa)
        public List<DashboardPrevisaoDto> PrevisaoProximosMeses { get; set; } = new();
    }

    public class DashboardCategoriaDto
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }

    // [NOVO] DTO para alimentar o gráfico de colunas no Blazor
    public class DashboardPrevisaoDto
    {
        public string Mes { get; set; } = string.Empty;
        public decimal Receitas { get; set; } // [NOVO]
        public decimal Despesas { get; set; } // [NOVO]
        public decimal SaldoPrevisto { get; set; }
    }
}