using MyFinance.Domain.Entities;

namespace MyFinance.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> GetByIdAsync(Guid id);
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task AddAsync(Categoria categoria); 
        void Update(Categoria categoria);
        void Delete(Categoria categoria);
    }
}