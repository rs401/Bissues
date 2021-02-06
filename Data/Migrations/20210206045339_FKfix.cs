using Microsoft.EntityFrameworkCore.Migrations;

namespace Bissues.Data.Migrations
{
    public partial class FKfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_OwnerId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Bissues_BissueId",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_OwnerId",
                table: "Notifications",
                newName: "IX_Notifications_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_BissueId",
                table: "Notifications",
                newName: "IX_Notifications_BissueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_OwnerId",
                table: "Notifications",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Bissues_BissueId",
                table: "Notifications",
                column: "BissueId",
                principalTable: "Bissues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_OwnerId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Bissues_BissueId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_OwnerId",
                table: "Notification",
                newName: "IX_Notification_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_BissueId",
                table: "Notification",
                newName: "IX_Notification_BissueId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_OwnerId",
                table: "Notification",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Bissues_BissueId",
                table: "Notification",
                column: "BissueId",
                principalTable: "Bissues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
