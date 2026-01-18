using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure.Data;

namespace MyFinance.Infrastructure.Repositories
{
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly AppDbContext _context;

        // Injeção de Dependência: O 'context' chega pronto aqui no construtor
        public LancamentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Lancamento lancamento)
        {
            // Adiciona na memória do EF
            await _context.Lancamentos.AddAsync(lancamento);

            // Comita no banco de dados (SQL)
            //await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Lancamento>> GetByContaIdAsync(Guid contaId)
        {
            return await _context.Lancamentos
                .Where(x => x.ContaId == contaId)
                .OrderByDescending(x => x.DataVencimento) // Mais recentes primeiro
                .ToListAsync();
        }
    }
}