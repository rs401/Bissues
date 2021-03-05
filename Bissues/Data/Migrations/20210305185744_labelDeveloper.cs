using Microsoft.EntityFrameworkCore.Migrations;

namespace Bissues.Migrations
{
    public partial class labelDeveloper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedDeveloperId",
                table: "Bissues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Label",
                table: "Bissues",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedDeveloperId",
                table: "Bissues");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Bissues");
        }
    }
}
