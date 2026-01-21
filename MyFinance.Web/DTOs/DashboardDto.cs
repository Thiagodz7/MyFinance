namespace MyFinance.Web.DTOs
{
    public class DashboardDto
    {
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal SaldoTotal { get; set; }

        // Para o Gráfico de Rosca (Top categorias de despesa)
        public List<DashboardCategoriaDto> DespesasPorCategoria { get; set; } = new();
    }

    public class DashboardCategoriaDto
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal Valor { get; set; } // Valor absoluto (sem sinal negativo)
    }
}