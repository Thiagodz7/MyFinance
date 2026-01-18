namespace MyFinance.Domain.Events
{
    // É uma classe simples (POCO), só para carregar dados
    public class LancamentoCriadoEvent
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataOcorrencia { get; set; }

        // [NOVO] O dado crucial para o Worker
        public Guid ContaId { get; set; }
    }
}