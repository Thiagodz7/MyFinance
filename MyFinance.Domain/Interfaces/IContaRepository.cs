using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface IContaRepository
    {
        Task<Conta> GetByIdAsync(Guid id);
        Task UpdateAsync(Conta conta);
        Task AddAsync(Conta conta); // Pra gente criar uma conta inicial
        Task<IEnumerable<Conta>> GetAllAsync();
    }
}