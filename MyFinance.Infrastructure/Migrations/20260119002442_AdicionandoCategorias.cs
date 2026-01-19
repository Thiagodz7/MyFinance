using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<Guid>(
            //    name: "CategoriaId",
            //    table: "Lancamentos",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            // ==============================================================================
            // [SOLUÇÃO ENTERPRISE] 
            // Vou Criar uma Categoria Padrão para suprir os dados antigos
            // ==============================================================================

            var categoriaPadraoId = Guid.NewGuid(); // Geramos um ID aqui

            // Inserimos a conta no banco na marra
            migrationBuilder.Sql($@"
                INSERT INTO Categorias (Id, Nome, Tipo, DataCriacao) 
                VALUES ('{categoriaPadraoId}', 'Categoria Padrão', 1, GETDATE())
            ");

            // ==============================================================================

            // 2. Agora criamos a coluna na tabela Lancamentos
            // O SEGREDO: Usamos 'defaultValue' apontando para a conta que acabamos de criar via SQL
            migrationBuilder.AddColumn<Guid>(
                name: "CategoriaId",
                table: "Lancamentos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: categoriaPadraoId); // <--- AQUI ESTÁ A MÁGICA

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_CategoriaId",
                table: "Lancamentos",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Categorias_CategoriaId",
                table: "Lancamentos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Categorias_CategoriaId",
                table: "Lancamentos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Lancamentos_CategoriaId",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Lancamentos");
        }
    }
}
