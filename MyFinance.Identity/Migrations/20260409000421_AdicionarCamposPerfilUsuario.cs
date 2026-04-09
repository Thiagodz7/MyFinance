using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinance.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposPerfilUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ocupacao",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PlanoAtual",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TetoGastosMensal",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenAssinatura",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ocupacao",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PlanoAtual",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TetoGastosMensal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TokenAssinatura",
                table: "AspNetUsers");
        }
    }
}
