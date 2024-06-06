using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacaoWeb.Migrations
{
    public partial class CreateMaping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Filmes",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Filmes",
                newName: "Titulo");

            migrationBuilder.CreateIndex(
                name: "IX_Filmes_Sinopse",
                table: "Filmes",
                column: "Sinopse",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Filmes_Sinopse",
                table: "Filmes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Filmes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Titulo",
                table: "Filmes",
                newName: "Title");
        }
    }
}
