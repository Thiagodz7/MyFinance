using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure.Data;

namespace MyFinance.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Categoria?> GetByIdAsync(Guid id)
        {
            return await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            // Ordenar por nome facilita a vida do usuário
            return await _context.Categorias.AsNoTracking().OrderBy(c => c.Nome).ToListAsync();
        }

        public async Task AddAsync(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
            // Lembra? Não damos SaveChanges aqui porque o UnitOfWork cuida disso!
        }

        public void Update(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
        }

        public Task UpdateAsync(Guid id, Categoria categoria, CancellationToken ct)
        {
            _context.Categorias.Update(categoria);

            return Task.CompletedTask;
        }
        public void Delete(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
        }
    }
}