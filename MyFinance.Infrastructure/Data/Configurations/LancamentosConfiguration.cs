using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinance.Domain.Entities;

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
