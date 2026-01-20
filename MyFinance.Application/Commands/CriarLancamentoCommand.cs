using MediatR;

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
    }
}