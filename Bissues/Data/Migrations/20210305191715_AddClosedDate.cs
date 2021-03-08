using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bissues.Migrations
{
    public partial class AddClosedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedDate",
                table: "Bissues",
                type: "timestamp without time zone",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedDate",
                table: "Bissues");
        }
    }
}
