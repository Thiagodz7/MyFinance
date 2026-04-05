using MediatR;
using MyFinance.Domain.Entities;
using static MyFinance.Domain.Entities.Lancamento;

namespace MyFinance.Application.Commands
{
    // IRequest<Guid>: Diz que esse comando vai devolver um Guid (o ID do lançamento criado)
    public class CriarLancamentoCommand : IRequest<Guid>
    {
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public Guid ContaId { get; set; }
        public Guid CategoriaId { get; set; }

        // [NOVOS] Campos para o motor de recorrência
        public bool EhRecorrente { get; set; }
        public TipoFrequencia Frequencia { get; set; }
        public int TotalParcelas { get; set; } = 1;
    }
}