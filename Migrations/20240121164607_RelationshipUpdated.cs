using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSignalR.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chats_ChatID",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ChatID",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ChatID",
                table: "Users",
                newName: "IX_Users_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chats_UserId",
                table: "Users",
                column: "UserId",
                principalTable: "Chats",
                principalColumn: "ChatID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chats_UserId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "ChatID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserId",
                table: "Users",
                newName: "IX_Users_ChatID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chats_ChatID",
                table: "Users",
                column: "ChatID",
                principalTable: "Chats",
                principalColumn: "ChatID");
        }
    }
}
