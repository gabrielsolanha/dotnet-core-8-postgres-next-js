using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacaoWeb.Migrations
{
    public partial class dacpac019026587b767f2f967e7dd43fadc3c9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Rodagem",
                table: "Filmes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Screen_Name",
                table: "Screen",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Screen_Name",
                table: "Screen");

            migrationBuilder.DropColumn(
                name: "Rodagem",
                table: "Filmes");
        }
    }
}
