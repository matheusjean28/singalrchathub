using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSignalR.Migrations
{
    /// <inheritdoc />
    public partial class PermissionLevelAddAtDBContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissionData_Chats_ChatID",
                table: "UserPermissionData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermissionData",
                table: "UserPermissionData");

            migrationBuilder.RenameTable(
                name: "UserPermissionData",
                newName: "UserPermission");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermissionData_ChatID",
                table: "UserPermission",
                newName: "IX_UserPermission_ChatID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermission",
                table: "UserPermission",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_Chats_ChatID",
                table: "UserPermission",
                column: "ChatID",
                principalTable: "Chats",
                principalColumn: "ChatID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_Chats_ChatID",
                table: "UserPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPermission",
                table: "UserPermission");

            migrationBuilder.RenameTable(
                name: "UserPermission",
                newName: "UserPermissionData");

            migrationBuilder.RenameIndex(
                name: "IX_UserPermission_ChatID",
                table: "UserPermissionData",
                newName: "IX_UserPermissionData_ChatID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPermissionData",
                table: "UserPermissionData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissionData_Chats_ChatID",
                table: "UserPermissionData",
                column: "ChatID",
                principalTable: "Chats",
                principalColumn: "ChatID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
