using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface ILancamentoRepository
    {
        Task AddAsync(Lancamento lancamento);
        Task<IEnumerable<Lancamento>> GetByContaIdAsync(Guid contaId);
        Task<IEnumerable<Lancamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
        void Deletar(Lancamento lancamento);
        void Update(Lancamento lancamento);
        Task<Lancamento?> GetByIdAsync(Guid id);

        Task AddRangeAsync(IEnumerable<Lancamento> lancamentos);
        Task<IEnumerable<Lancamento>> ObterPorGrupoIdAsync(Guid grupoId);
        void DeletarVarios(IEnumerable<Lancamento> lancamentos);

        Task<List<Lancamento>> GetAllAsync();
    }
}