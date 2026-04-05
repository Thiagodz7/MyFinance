using MediatR;

namespace MyFinance.Application.Commands
{
    public enum TipoExclusaoRecorrencia
    {
        ApenasEste = 0,
        EsteEProximos = 1,
        TodosDoGrupo = 2
    }

    public class DeletarLancamentoCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public TipoExclusaoRecorrencia TipoExclusao { get; set; }

        public DeletarLancamentoCommand(Guid id, TipoExclusaoRecorrencia tipoExclusao = TipoExclusaoRecorrencia.ApenasEste)
        {
            Id = id;
            TipoExclusao = tipoExclusao;
        }
    }
}
