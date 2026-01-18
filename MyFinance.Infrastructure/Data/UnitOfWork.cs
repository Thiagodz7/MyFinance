using MyFinance.Domain.Interfaces;
using MyFinance.Infrastructure.Data;

namespace MyFinance.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            // O Pulo do Gato: É aqui que a mágica acontece.
            // Todas as alterações feitas pelos repositórios serão salvas numa única transação.
            await _context.SaveChangesAsync();
        }
    }
}