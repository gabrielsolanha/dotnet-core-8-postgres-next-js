using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacaoWeb.Migrations
{
    public partial class dacpac0190285cc5377a81b345bc4100ba62b9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessLevel",
                table: "UserScreen",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_UserName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AccessLevel",
                table: "UserScreen");
        }
    }
}
