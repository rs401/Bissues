using Microsoft.EntityFrameworkCore.Migrations;

namespace Bissues.Migrations
{
    public partial class FinalRefactorIHopePart2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeToo_Bissues_BissueId",
                table: "MeToo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeToo",
                table: "MeToo");

            migrationBuilder.RenameTable(
                name: "MeToo",
                newName: "MeToos");

            migrationBuilder.RenameIndex(
                name: "IX_MeToo_BissueId",
                table: "MeToos",
                newName: "IX_MeToos_BissueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeToos",
                table: "MeToos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeToos_Bissues_BissueId",
                table: "MeToos",
                column: "BissueId",
                principalTable: "Bissues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeToos_Bissues_BissueId",
                table: "MeToos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeToos",
                table: "MeToos");

            migrationBuilder.RenameTable(
                name: "MeToos",
                newName: "MeToo");

            migrationBuilder.RenameIndex(
                name: "IX_MeToos_BissueId",
                table: "MeToo",
                newName: "IX_MeToo_BissueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeToo",
                table: "MeToo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeToo_Bissues_BissueId",
                table: "MeToo",
                column: "BissueId",
                principalTable: "Bissues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
