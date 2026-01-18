using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using System.Reflection;

namespace MyFinance.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // Construtor que recebe as opções (ex: string de conexão)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Representação das tabelas
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<Conta> Contas { get; set; } // <--- Adicione isso
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Essa linha mágica carrega todas as configurações (como a LancamentoConfiguration) 
            // que estiverem neste projeto. Assim você não precisa adicionar uma por uma.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}