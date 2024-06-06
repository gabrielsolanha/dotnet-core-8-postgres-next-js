using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AplicacaoWeb.Migrations
{
    public partial class CreateBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Filmes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Filmes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Filmes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Filmes",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Filmes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Filmes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Filmes");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Filmes");
        }
    }
}
