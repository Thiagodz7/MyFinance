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
                .AsNoTracking()
                .Include(l => l.Categoria)
                .OrderByDescending(x => x.DataVencimento) // Mais recentes primeiro
                .ToListAsync();
        }

        public async Task<IEnumerable<Lancamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _context.Lancamentos
                .AsNoTracking()
                .Include(l => l.Categoria) // <--- O PULO DO GATO: Faz o JOIN no SQL
                .Where(l => l.DataVencimento >= inicio && l.DataVencimento <= fim)
                .ToListAsync();
        }
    }
}