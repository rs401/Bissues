using Microsoft.EntityFrameworkCore.Migrations;

namespace Bissues.Data.Migrations
{
    public partial class OwnerId2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bissues_AspNetUsers_AppUserId",
                table: "Bissues");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_AppUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_AppUserId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Notifications",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_AppUserId",
                table: "Notifications",
                newName: "IX_Notifications_OwnerId");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Messages",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_AppUserId",
                table: "Messages",
                newName: "IX_Messages_OwnerId");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Bissues",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Bissues_AppUserId",
                table: "Bissues",
                newName: "IX_Bissues_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "AppUser",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUser",
                table: "Notifications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUser",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUser",
                table: "Bissues",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OwnerId",
                table: "Projects",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bissues_AspNetUsers_OwnerId",
                table: "Bissues",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_OwnerId",
                table: "Messages",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_OwnerId",
                table: "Notifications",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_OwnerId",
                table: "Projects",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bissues_AspNetUsers_OwnerId",
                table: "Bissues");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_OwnerId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_OwnerId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_OwnerId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OwnerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AppUser",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AppUser",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AppUser",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "AppUser",
                table: "Bissues");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Notifications",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_OwnerId",
                table: "Notifications",
                newName: "IX_Notifications_AppUserId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Messages",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_OwnerId",
                table: "Messages",
                newName: "IX_Messages_AppUserId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Bissues",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bissues_OwnerId",
                table: "Bissues",
                newName: "IX_Bissues_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bissues_AspNetUsers_AppUserId",
                table: "Bissues",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_AppUserId",
                table: "Messages",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_AppUserId",
                table: "Notifications",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
