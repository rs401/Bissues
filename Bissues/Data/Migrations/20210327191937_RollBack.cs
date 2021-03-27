using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Bissues.Migrations
{
    public partial class RollBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeTooIp");

            migrationBuilder.DropTable(
                name: "MeToo");

            migrationBuilder.AddColumn<List<string>>(
                name: "MeToos",
                table: "Bissues",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                    BissueId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeToo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeToo_Bissues_BissueId",
                        column: x => x.BissueId,
                        principalTable: "Bissues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeTooIp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BissueId = table.Column<int>(type: "integer", nullable: false),
                    Ip = table.Column<string>(type: "text", nullable: true),
                    MeTooId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeTooIp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeTooIp_Bissues_BissueId",
                        column: x => x.BissueId,
                        principalTable: "Bissues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeTooIp_MeToo_MeTooId",
                        column: x => x.MeTooId,
                        principalTable: "MeToo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeToo_BissueId",
                table: "MeToo",
                column: "BissueId");

            migrationBuilder.CreateIndex(
                name: "IX_MeTooIp_BissueId",
                table: "MeTooIp",
                column: "BissueId");

            migrationBuilder.CreateIndex(
                name: "IX_MeTooIp_Ip_BissueId",
                table: "MeTooIp",
                columns: new[] { "Ip", "BissueId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeTooIp_MeTooId",
                table: "MeTooIp",
                column: "MeTooId");
        }
    }
}
