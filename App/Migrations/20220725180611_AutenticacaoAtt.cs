using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Migrations
{
    public partial class AutenticacaoAtt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFuncionario",
                table: "Autenticacoes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFuncionario",
                table: "Autenticacoes");
        }
    }
}
