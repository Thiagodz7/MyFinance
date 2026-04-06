namespace MyFinance.Web.DTOs
{
    public class DashboardDto
    {
        public string Id { get; set; } = "dashboard_atual";
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal SaldoTotal { get; set; }

        public List<DashboardCategoriaDto> DespesasPorCategoria { get; set; } = new();

        // [NOVO] Para o Gráfico de Barras (Previsibilidade)
        public List<DashboardPrevisaoDto> PrevisaoProximosMeses { get; set; } = new();
        public decimal LucroPrevistoAno { get; set; }
    }

    public class DashboardCategoriaDto
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }

    // [NOVO]
    public class DashboardPrevisaoDto
    {
        public string Mes { get; set; } = string.Empty;
        public decimal Receitas { get; set; } // [NOVO]
        public decimal Despesas { get; set; } // [NOVO]
        public decimal SaldoPrevisto { get; set; }
    }
}