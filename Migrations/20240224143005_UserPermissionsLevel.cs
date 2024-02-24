using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSignalR.Migrations
{
    /// <inheritdoc />
    public partial class UserPermissionsLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chats_UserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserPermissionLevel",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChatUser",
                columns: table => new
                {
                    ChatID = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUser", x => new { x.ChatID, x.UserId });
                    table.ForeignKey(
                        name: "FK_ChatUser_Chats_UserId",
                        column: x => x.UserId,
                        principalTable: "Chats",
                        principalColumn: "ChatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUser_Users_ChatID",
                        column: x => x.ChatID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_UserId",
                table: "ChatUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatUser");

            migrationBuilder.DropColumn(
                name: "UserPermissionLevel",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chats_UserId",
                table: "Users",
                column: "UserId",
                principalTable: "Chats",
                principalColumn: "ChatID");
        }
    }
}
