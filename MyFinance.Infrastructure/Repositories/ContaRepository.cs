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

        public async Task<Conta> GetByIdAsync(Guid id)
        {
            return await _context.Contas.FindAsync(id);
        }

        public async Task AddAsync(Conta conta)
        {
            await _context.Contas.AddAsync(conta);
            //await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Conta conta)
        {
            _context.Contas.Update(conta);
            //await _context.SaveChangesAsync();
        }
    }
}