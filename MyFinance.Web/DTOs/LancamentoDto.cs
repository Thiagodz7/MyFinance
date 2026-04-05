namespace MyFinance.Web.DTOs
{
    public class LancamentoDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public Guid CategoriaId { get; set; }
        public Guid ContaId { get; set; }

        // [NOVOS] Campos de Recorrência
        public bool EhRecorrente { get; set; }
        public int Frequencia { get; set; }
        public int ParcelaAtual { get; set; }
        public int TotalParcelas { get; set; }
        public Guid? GrupoRecorrenciaId { get; set; }
    }
}