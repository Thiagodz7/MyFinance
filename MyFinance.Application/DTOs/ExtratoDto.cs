namespace MyFinance.Application.DTOs
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

    public class LancamentoDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; } = string.Empty; // "Receita" ou "Despesa"
        public string Categoria { get; set; } = string.Empty;
        public Guid ContaId { get; set; }
        public Guid CategoriaId { get; set; }
        public bool Pago { get; set; } // Adicionado pois você precisará saber na UI se já foi compensado


        // --- [NOVOS CAMPOS PARA O FRONT-END] ---
        public bool EhRecorrente { get; set; }
        public int Frequencia { get; set; } // Enviamos como Int para facilitar o bind no MudSelect do Blazor
        public int ParcelaAtual { get; set; }
        public int TotalParcelas { get; set; }
        public Guid? GrupoRecorrenciaId { get; set; }
    }
}