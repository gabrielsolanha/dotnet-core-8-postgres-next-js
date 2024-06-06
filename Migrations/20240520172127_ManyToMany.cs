using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AplicacaoWeb.Migrations
{
    public partial class ManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Filmes",
                newName: "Ativo");

            migrationBuilder.RenameColumn(
                name: "Sinopse",
                table: "Filmes",
                newName: "Plate");

            migrationBuilder.RenameIndex(
                name: "IX_Filmes_Sinopse",
                table: "Filmes",
                newName: "IX_Filmes_Plate");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Filmes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "User",
                table: "Filmes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ImageUrlAndNames",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "text", nullable: false),
                    ArquiveName = table.Column<string>(type: "text", nullable: false),
                    FilmId = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageUrlAndNames", x => x.id);
                    table.ForeignKey(
                        name: "FK_ImageUrlAndNames_Filmes_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Filmes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Screen",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ScreenType = table.Column<string>(type: "text", nullable: false),
                    ScreenUrl = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screen", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: false),
                    CallMeName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserScreen",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ScreenId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserScreen", x => new { x.ScreenId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserScreen_Screen_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "Screen",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserScreen_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Filmes_User",
                table: "Filmes",
                column: "User");

            migrationBuilder.CreateIndex(
                name: "IX_ImageUrlAndNames_FilmId",
                table: "ImageUrlAndNames",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageUrlAndNames_Url",
                table: "ImageUrlAndNames",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Screen_ScreenUrl",
                table: "Screen",
                column: "ScreenUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Telefone",
                table: "User",
                column: "Telefone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserScreen_UserId",
                table: "UserScreen",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Filmes_User_User",
                table: "Filmes",
                column: "User",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Filmes_User_User",
                table: "Filmes");

            migrationBuilder.DropTable(
                name: "ImageUrlAndNames");

            migrationBuilder.DropTable(
                name: "UserScreen");

            migrationBuilder.DropTable(
                name: "Screen");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Filmes_User",
                table: "Filmes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Filmes");

            migrationBuilder.DropColumn(
                name: "User",
                table: "Filmes");

            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Filmes",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Plate",
                table: "Filmes",
                newName: "Sinopse");

            migrationBuilder.RenameIndex(
                name: "IX_Filmes_Plate",
                table: "Filmes",
                newName: "IX_Filmes_Sinopse");
        }
    }
}
