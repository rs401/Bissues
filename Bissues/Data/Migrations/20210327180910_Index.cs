using Microsoft.EntityFrameworkCore.Migrations;

namespace Bissues.Migrations
{
    public partial class Index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BissueId",
                table: "MeTooIp",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MeTooIp_BissueId",
                table: "MeTooIp",
                column: "BissueId");

            migrationBuilder.CreateIndex(
                name: "IX_MeTooIp_Ip_BissueId",
                table: "MeTooIp",
                columns: new[] { "Ip", "BissueId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MeTooIp_Bissues_BissueId",
                table: "MeTooIp",
                column: "BissueId",
                principalTable: "Bissues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeTooIp_Bissues_BissueId",
                table: "MeTooIp");

            migrationBuilder.DropIndex(
                name: "IX_MeTooIp_BissueId",
                table: "MeTooIp");

            migrationBuilder.DropIndex(
                name: "IX_MeTooIp_Ip_BissueId",
                table: "MeTooIp");

            migrationBuilder.DropColumn(
                name: "BissueId",
                table: "MeTooIp");
        }
    }
}
