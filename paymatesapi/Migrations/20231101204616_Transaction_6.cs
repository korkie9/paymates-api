using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace paymatesapi.Migrations
{
    /// <inheritdoc />
    public partial class Transaction_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Friends_FriendPairFriendId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "FriendPairFriendId",
                table: "Transactions",
                newName: "FriendId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_FriendPairFriendId",
                table: "Transactions",
                newName: "IX_Transactions_FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Friends_FriendId",
                table: "Transactions",
                column: "FriendId",
                principalTable: "Friends",
                principalColumn: "FriendId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Friends_FriendId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "FriendId",
                table: "Transactions",
                newName: "FriendPairFriendId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_FriendId",
                table: "Transactions",
                newName: "IX_Transactions_FriendPairFriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Friends_FriendPairFriendId",
                table: "Transactions",
                column: "FriendPairFriendId",
                principalTable: "Friends",
                principalColumn: "FriendId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
