namespace MyFinance.Application.DTOs
{
    public class ExtratoDto
    {
        public Guid ContaId { get; set; }
        public string NomeConta { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;
        public decimal SaldoAtual { get; set; }
        public List<LancamentoDto> Lancamentos { get; set; } = new List<LancamentoDto>();
    }

    public class LancamentoDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; } = string.Empty; // "Receita" ou "Despesa"
        public string Categoria { get; set; } = string.Empty;
    }
}