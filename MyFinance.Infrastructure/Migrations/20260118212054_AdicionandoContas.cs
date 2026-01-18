using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoContas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<Guid>(
            //    name: "ContaId",
            //    table: "Lancamentos",
            //    type: "uniqueidentifier",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Contas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SaldoAtual = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contas", x => x.Id);
                });


            // ==============================================================================
            // [INTERVENÇÃO MANUAL - SOLUÇÃO ENTERPRISE] 
            // Vamos criar uma conta padrão via SQL Direto para salvar os dados antigos
            // ==============================================================================

            var contaPadraoId = Guid.NewGuid(); // Geramos um ID aqui

            // Inserimos a conta no banco na marra
            migrationBuilder.Sql($@"
        INSERT INTO Contas (Id, Nome, SaldoAtual, DataCriacao) 
        VALUES ('{contaPadraoId}', 'Conta Padrão (Migração)', 0, GETDATE())
    ");

            // ==============================================================================

            // 2. Agora criamos a coluna na tabela Lancamentos
            // O SEGREDO: Usamos 'defaultValue' apontando para a conta que acabamos de criar via SQL
            migrationBuilder.AddColumn<Guid>(
                name: "ContaId",
                table: "Lancamentos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: contaPadraoId); // <--- AQUI ESTÁ A MÁGICA


            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_ContaId",
                table: "Lancamentos",
                column: "ContaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lancamentos_Contas_ContaId",
                table: "Lancamentos",
                column: "ContaId",
                principalTable: "Contas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lancamentos_Contas_ContaId",
                table: "Lancamentos");

            migrationBuilder.DropTable(
                name: "Contas");

            migrationBuilder.DropIndex(
                name: "IX_Lancamentos_ContaId",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "ContaId",
                table: "Lancamentos");
        }
    }
}
