using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bissues.Migrations
{
    public partial class FinalRefactorIHope : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeToos",
                table: "Bissues");

            migrationBuilder.CreateTable(
                name: "MeToo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ip = table.Column<string>(type: "text", nullable: true),
                    BissueId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeToo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeToo_Bissues_BissueId",
                        column: x => x.BissueId,
                        principalTable: "Bissues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeToo_BissueId",
                table: "MeToo",
                column: "BissueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeToo");

            migrationBuilder.AddColumn<string[]>(
                name: "MeToos",
                table: "Bissues",
                type: "text[]",
                nullable: true);
        }
    }
}
