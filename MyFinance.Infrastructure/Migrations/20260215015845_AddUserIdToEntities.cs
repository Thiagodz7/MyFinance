using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFinance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Lancamentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "adb17f3f-498b-4e27-822f-e3cb4aeaa768");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Contas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "adb17f3f-498b-4e27-822f-e3cb4aeaa768");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Categorias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "adb17f3f-498b-4e27-822f-e3cb4aeaa768");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Contas");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Categorias");
        }
    }
}
