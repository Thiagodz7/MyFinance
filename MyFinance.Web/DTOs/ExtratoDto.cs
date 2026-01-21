namespace MyFinance.Web.DTOs
{
    public class ExtratoDto
    {
        public Guid ContaId { get; set; }
        public string NomeConta { get; set; } = string.Empty;
        public decimal SaldoAtual { get; set; }
        public List<LancamentoDto> Lancamentos { get; set; } = new();
    }
}