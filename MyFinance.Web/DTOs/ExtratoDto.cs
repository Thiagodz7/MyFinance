namespace MyFinance.Web.DTOs
{
    public class ExtratoDto
    {
        public Guid ContaId { get; set; }
        public string NomeConta { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;

        public decimal SaldoAnterior { get; set; } 
        public decimal LucroDoMes { get; set; } 
        public decimal SaldoAtual { get; set; }
        public decimal SaldoReal { get; set; }


        public List<LancamentoDto> Lancamentos { get; set; } = new List<LancamentoDto>();
    }

}