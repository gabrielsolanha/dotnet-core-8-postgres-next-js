using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacaoWeb.Migrations
{
    public partial class dacpac018fe60d37aa7346b99968b3c8df01d2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Filmes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Filmes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
