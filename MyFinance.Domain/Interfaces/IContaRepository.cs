using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface IContaRepository
    {
        Task<Conta> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, Conta conta, CancellationToken cancellationToken);
        Task AddAsync(Conta conta, CancellationToken cancellationToken); // Pra gente criar uma conta inicial
        Task<IEnumerable<Conta>> GetAllAsync();
    }
}