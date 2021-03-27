using Microsoft.EntityFrameworkCore.Migrations;

namespace Bissues.Migrations
{
    public partial class FinalRefactorIHopePart3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeToos_Bissues_BissueId",
                table: "MeToos");

            migrationBuilder.AlterColumn<int>(
                name: "BissueId",
                table: "MeToos",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MeToos_Bissues_BissueId",
                table: "MeToos",
                column: "BissueId",
                principalTable: "Bissues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeToos_Bissues_BissueId",
                table: "MeToos");

            migrationBuilder.AlterColumn<int>(
                name: "BissueId",
                table: "MeToos",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_MeToos_Bissues_BissueId",
                table: "MeToos",
                column: "BissueId",
                principalTable: "Bissues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
