using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIChatbot.Migrations
{
    /// <inheritdoc />
    public partial class RenamedChatroomToChatrooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chatroom_users_user_id",
                table: "chatroom");

            migrationBuilder.DropForeignKey(
                name: "FK_messages_chatroom_chatroom_id",
                table: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chatroom",
                table: "chatroom");

            migrationBuilder.RenameTable(
                name: "chatroom",
                newName: "chatrooms");

            migrationBuilder.RenameIndex(
                name: "IX_chatroom_user_id",
                table: "chatrooms",
                newName: "IX_chatrooms_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chatrooms",
                table: "chatrooms",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_chatrooms_users_user_id",
                table: "chatrooms",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_messages_chatrooms_chatroom_id",
                table: "messages",
                column: "chatroom_id",
                principalTable: "chatrooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chatrooms_users_user_id",
                table: "chatrooms");

            migrationBuilder.DropForeignKey(
                name: "FK_messages_chatrooms_chatroom_id",
                table: "messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chatrooms",
                table: "chatrooms");

            migrationBuilder.RenameTable(
                name: "chatrooms",
                newName: "chatroom");

            migrationBuilder.RenameIndex(
                name: "IX_chatrooms_user_id",
                table: "chatroom",
                newName: "IX_chatroom_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chatroom",
                table: "chatroom",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_chatroom_users_user_id",
                table: "chatroom",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_messages_chatroom_chatroom_id",
                table: "messages",
                column: "chatroom_id",
                principalTable: "chatroom",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
