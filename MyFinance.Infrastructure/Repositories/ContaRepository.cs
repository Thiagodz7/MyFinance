using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure.Data;

namespace MyFinance.Infrastructure.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly AppDbContext _context;

        public ContaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Adicione o método
        public async Task<IEnumerable<Conta>> GetAllAsync()
        {
            return await _context.Contas.AsNoTracking().ToListAsync();
        }

        public async Task<Conta> GetByIdAsync(Guid id)
        {
            return await _context.Contas.FindAsync(id);
        }

        public async Task AddAsync(Conta conta, CancellationToken cancellationToken)
        {
            await _context.Contas.AddAsync(conta, cancellationToken);
            //await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Guid id, Conta conta, CancellationToken cancellationToken)
        {
            _context.Contas.Update(conta);

            return Task.CompletedTask;
        }
    }
}