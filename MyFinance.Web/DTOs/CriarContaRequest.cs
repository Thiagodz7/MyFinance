namespace MyFinance.Web.DTOs
{
    public class CriarContaRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;
        public decimal SaldoInicial { get; set; }
    }
}