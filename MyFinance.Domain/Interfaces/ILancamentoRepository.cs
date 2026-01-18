using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface ILancamentoRepository
    {
        // Usamos Task para operações assíncronas (async/await)
        Task AddAsync(Lancamento lancamento);
        Task<IEnumerable<Lancamento>> GetByContaIdAsync(Guid contaId);
    }
}