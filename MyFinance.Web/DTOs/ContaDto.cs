namespace MyFinance.Web.DTOs
{
    public class ContaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal SaldoAtual { get; set; }
    }
}
