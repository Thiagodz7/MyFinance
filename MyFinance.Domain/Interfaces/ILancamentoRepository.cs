using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface ILancamentoRepository
    {
        // Usamos Task para operações assíncronas (async/await)
        Task AddAsync(Lancamento lancamento);
        Task<IEnumerable<Lancamento>> GetByContaIdAsync(Guid contaId);
        Task<IEnumerable<Lancamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
        void Deletar(Lancamento lancamento);
        void Update(Lancamento lancamento);
        Task<Lancamento?> GetByIdAsync(Guid id);

        // [NOVO] Adicionado para salvar recorrências em lote com alta performance
        Task AddRangeAsync(IEnumerable<Lancamento> lancamentos);
        Task<IEnumerable<Lancamento>> ObterPorGrupoIdAsync(Guid grupoId);
        void DeletarVarios(IEnumerable<Lancamento> lancamentos);
    }
}