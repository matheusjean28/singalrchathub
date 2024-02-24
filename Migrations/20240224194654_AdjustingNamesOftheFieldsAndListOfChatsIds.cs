using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSignalR.Migrations
{
    /// <inheritdoc />
    public partial class AdjustingNamesOftheFieldsAndListOfChatsIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_UserId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Chats_UserId",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Users_ChatID",
                table: "ChatUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ChatUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "ChatID",
                table: "ChatUser",
                newName: "MyOwnsChatIdsChatID");

            migrationBuilder.RenameIndex(
                name: "IX_ChatUser_UserId",
                table: "ChatUser",
                newName: "IX_ChatUser_UsersId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Chats",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_UserId",
                table: "Chats",
                newName: "IX_Chats_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_OwnerId",
                table: "Chats",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Chats_MyOwnsChatIdsChatID",
                table: "ChatUser",
                column: "MyOwnsChatIdsChatID",
                principalTable: "Chats",
                principalColumn: "ChatID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Users_UsersId",
                table: "ChatUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_OwnerId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Chats_MyOwnsChatIdsChatID",
                table: "ChatUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatUser_Users_UsersId",
                table: "ChatUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ChatUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "MyOwnsChatIdsChatID",
                table: "ChatUser",
                newName: "ChatID");

            migrationBuilder.RenameIndex(
                name: "IX_ChatUser_UsersId",
                table: "ChatUser",
                newName: "IX_ChatUser_UserId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Chats",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_OwnerId",
                table: "Chats",
                newName: "IX_Chats_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_UserId",
                table: "Chats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Chats_UserId",
                table: "ChatUser",
                column: "UserId",
                principalTable: "Chats",
                principalColumn: "ChatID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUser_Users_ChatID",
                table: "ChatUser",
                column: "ChatID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
