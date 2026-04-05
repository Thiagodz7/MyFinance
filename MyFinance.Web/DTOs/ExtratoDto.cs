namespace MyFinance.Web.DTOs
{
    public class ExtratoDto
    {
        public Guid ContaId { get; set; }
        public string NomeConta { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;

        // --- O TRIPÉ FINANCEIRO ---
        public decimal SaldoAnterior { get; set; } // O que sobrou do mês passado
        public decimal LucroDoMes { get; set; } // O que entrou e saiu só neste mês
        public decimal SaldoAtual { get; set; } // Saldo final da conta

        public List<LancamentoDto> Lancamentos { get; set; } = new List<LancamentoDto>();
    }

}