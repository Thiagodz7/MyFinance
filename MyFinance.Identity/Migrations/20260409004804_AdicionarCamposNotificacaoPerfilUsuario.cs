using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinance.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposNotificacaoPerfilUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotificarEmail",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotificarPush",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotificarTelefone",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NotificarWhatsapp",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificarEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NotificarPush",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NotificarTelefone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NotificarWhatsapp",
                table: "AspNetUsers");
        }
    }
}
