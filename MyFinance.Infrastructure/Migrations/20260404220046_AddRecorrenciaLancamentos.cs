using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecorrenciaLancamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EhRecorrente",
                table: "Lancamentos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Frequencia",
                table: "Lancamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "GrupoRecorrenciaId",
                table: "Lancamentos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParcelaAtual",
                table: "Lancamentos",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "TotalParcelas",
                table: "Lancamentos",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_GrupoRecorrenciaId",
                table: "Lancamentos",
                column: "GrupoRecorrenciaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lancamentos_GrupoRecorrenciaId",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "EhRecorrente",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "Frequencia",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "GrupoRecorrenciaId",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "ParcelaAtual",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "TotalParcelas",
                table: "Lancamentos");
        }
    }
}
