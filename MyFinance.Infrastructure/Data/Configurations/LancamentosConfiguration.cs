using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;
using static MyFinance.Domain.Entities.Lancamento;

namespace MyFinance.Infrastructure.Data.Configurations
{
    public class LancamentoConfiguration : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            // Nome da tabela no SQL
            builder.ToTable("Lancamentos");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Propriedades
            builder.Property(x => x.Descricao)
                .IsRequired() // NOT NULL
                .HasMaxLength(100); // VARCHAR(100)

            builder.Property(x => x.Valor)
                .HasPrecision(18, 2); // DECIMAL(18,2) - Importante para dinheiro!

            builder.Property(x => x.DataVencimento)
                .IsRequired();


            // ==========================================================
            // Configuração do Relacionamento (Foreign Key)
            // ==========================================================
            builder.Property(x => x.ContaId)
                .IsRequired(); // Garante que não pode ser nulo

            builder.HasOne(x => x.Conta)        // Um Lançamento tem UMA Conta
                   .WithMany()                  // Uma Conta tem MUITOS Lançamentos (não mapeamos a lista na volta, então fica vazio)
                   .HasForeignKey(x => x.ContaId) // A chave que liga eles é o ContaId
                   .OnDelete(DeleteBehavior.Restrict); // Segurança: Se tentar apagar a Conta, o banco BLOQUEIA se tiver lançamentos nela (pra não perder histórico)


            // ==========================================================
            // [NOVO] Configuração dos Campos de Recorrência
            // ==========================================================

            builder.Property(x => x.EhRecorrente)
                .IsRequired()
                .HasDefaultValue(false);

            // O EF Core salva Enums como inteiros no banco de dados por padrão.
            builder.Property(x => x.Frequencia)
                   .IsRequired()
                   .HasDefaultValue(TipoFrequencia.Nenhuma);

            builder.Property(x => x.ParcelaAtual)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(x => x.TotalParcelas)
                .IsRequired()
                .HasDefaultValue(1);

            // Nullable, pois lançamentos avulsos não pertencem a um grupo
            builder.Property(x => x.GrupoRecorrenciaId)
                .IsRequired(false);

            // [DICA DE PERFORMANCE] 
            // Índice para buscas ultra rápidas ao atualizar parcelas futuras
            builder.HasIndex(x => x.GrupoRecorrenciaId)
                .HasDatabaseName("IX_Lancamentos_GrupoRecorrenciaId");


            // ==========================================================   
            // Configuração do Relacionamento com Categoria (Foreign Key)
            // ==========================================================

            builder.Property(x => x.CategoriaId)
                .IsRequired();

            builder.HasOne(x => x.Categoria)
                    .WithMany()
                    .HasForeignKey(x => x.CategoriaId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
