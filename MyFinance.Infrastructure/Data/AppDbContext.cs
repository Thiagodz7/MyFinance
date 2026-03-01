using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using System.Reflection;

namespace MyFinance.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        // Construtor que recebe as opções (ex: string de conexão)
        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
              : base(options)
        {
            _currentUserService = currentUserService;
        }

        // Representação das tabelas
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Essa linha mágica carrega todas as configurações (como a LancamentoConfiguration) 
            // que estiverem neste projeto. Assim você não precisa adicionar uma por uma.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Conta>()
                .HasQueryFilter(x => x.UserId == _currentUserService.UserId);

            modelBuilder.Entity<Categoria>()
                .HasQueryFilter(x => x.UserId == _currentUserService.UserId);

            modelBuilder.Entity<Lancamento>()
                .HasQueryFilter(x => x.UserId == _currentUserService.UserId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // --- GRAVAÇÃO AUTOMÁTICA ---
            // Aqui corrigimos o erro de compilação usando a Interface

            // Troquei 'Entity' por 'IEntityComDono' no loop.
            // O ChangeTracker pega tudo, nós filtramos só o que tem dono.
            foreach (var entry in ChangeTracker.Entries<IEntityComDono>())
            {
                if (entry.State == EntityState.Added)
                {
                    // Chama o método público que criamos, respeitando o private set
                    entry.Entity.AssociarUsuario(_currentUserService.UserId);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}