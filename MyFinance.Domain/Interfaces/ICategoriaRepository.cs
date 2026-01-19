using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> GetByIdAsync(Guid id);
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task AddAsync(Categoria categoria); // Pra gente criar novas no futuro
    }
}