using MyFinance.Shared.Enums; 

namespace MyFinance.Shared.DTOs
{
    public class ContaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;
        public decimal SaldoAtual { get; set; }
        public bool Ativo { get; set; }

        public TipoConta Tipo { get; set; }
    }
}