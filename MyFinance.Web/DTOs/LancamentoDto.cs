namespace MyFinance.Web.DTOs
{
    public class LancamentoDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Tipo { get; set; } = string.Empty; // "Receita" ou "Despesa"
        public Guid CategoriaId { get; set; }
    }
}