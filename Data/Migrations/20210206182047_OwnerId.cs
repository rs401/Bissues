using Microsoft.EntityFrameworkCore.Migrations;

namespace Bissues.Data.Migrations
{
    public partial class OwnerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_Notifications_OwnerId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Messages_OwnerId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Bissues_OwnerId",
                table: "Bissues");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Bissues");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Notifications",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Messages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Bissues",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AppUserId",
                table: "Notifications",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AppUserId",
                table: "Messages",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bissues_AppUserId",
                table: "Bissues",
                column: "AppUserId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_Notifications_AppUserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Messages_AppUserId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Bissues_AppUserId",
                table: "Bissues");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Notifications",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Notifications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Messages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Messages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Bissues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Bissues",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_OwnerId",
                table: "Notifications",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_OwnerId",
                table: "Messages",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bissues_OwnerId",
                table: "Bissues",
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
        }
    }
}
