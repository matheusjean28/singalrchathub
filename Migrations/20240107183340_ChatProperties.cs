using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSignalR.Migrations
{
    /// <inheritdoc />
    public partial class ChatProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatID",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentChatId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentConnectionId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChatID",
                table: "Users",
                column: "ChatID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chats_ChatID",
                table: "Users",
                column: "ChatID",
                principalTable: "Chats",
                principalColumn: "ChatID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chats_ChatID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ChatID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentChatId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentConnectionId",
                table: "Users");
        }
    }
}
