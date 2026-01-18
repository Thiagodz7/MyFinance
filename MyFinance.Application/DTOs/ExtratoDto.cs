namespace MyFinance.Application.DTOs
{
    public class ExtratoDto
    {
        public Guid ContaId { get; set; }
        public string NomeConta { get; set; }
        public decimal SaldoAtual { get; set; }
        public List<LancamentoDto> Lancamentos { get; set; }
    }

    public class LancamentoDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; } // "Receita" ou "Despesa"
    }
}