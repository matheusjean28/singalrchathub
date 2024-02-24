using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSignalR.Migrations
{
    /// <inheritdoc />
    public partial class WrapperToChatListOfIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatUser");

            migrationBuilder.AddColumn<string>(
                name: "ChatID",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WrapperChat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ChatName = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WrapperChat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WrapperChat_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChatID",
                table: "Users",
                column: "ChatID");

            migrationBuilder.CreateIndex(
                name: "IX_WrapperChat_UserId",
                table: "WrapperChat",
                column: "UserId");

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

            migrationBuilder.DropTable(
                name: "WrapperChat");

            migrationBuilder.DropIndex(
                name: "IX_Users_ChatID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatID",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "ChatUser",
                columns: table => new
                {
                    MyOwnsChatIdsChatID = table.Column<string>(type: "TEXT", nullable: false),
                    UsersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUser", x => new { x.MyOwnsChatIdsChatID, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ChatUser_Chats_MyOwnsChatIdsChatID",
                        column: x => x.MyOwnsChatIdsChatID,
                        principalTable: "Chats",
                        principalColumn: "ChatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_UsersId",
                table: "ChatUser",
                column: "UsersId");
        }
    }
}
