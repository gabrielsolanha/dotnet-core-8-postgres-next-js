using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacaoWeb.Migrations
{
    public partial class dacpac01902bd417d47a2693ac3679e82001a1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "User",
                newName: "Inativo");

            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Screen",
                newName: "Inativo");

            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "ImageUrlAndNames",
                newName: "Inativo");

            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Filmes",
                newName: "Inativo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Inativo",
                table: "User",
                newName: "Ativo");

            migrationBuilder.RenameColumn(
                name: "Inativo",
                table: "Screen",
                newName: "Ativo");

            migrationBuilder.RenameColumn(
                name: "Inativo",
                table: "ImageUrlAndNames",
                newName: "Ativo");

            migrationBuilder.RenameColumn(
                name: "Inativo",
                table: "Filmes",
                newName: "Ativo");
        }
    }
}
