using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace paymatesapi.Migrations
{
    /// <inheritdoc />
    public partial class Transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Friends_FriendPairFriendOneUid_FriendPairFriendT~",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_FriendPairFriendOneUid_FriendPairFriendTwoUid",
                table: "Transactions",
                newName: "IX_Transactions_FriendPairFriendOneUid_FriendPairFriendTwoUid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Friends_FriendPairFriendOneUid_FriendPairFriend~",
                table: "Transactions",
                columns: new[] { "FriendPairFriendOneUid", "FriendPairFriendTwoUid" },
                principalTable: "Friends",
                principalColumns: new[] { "FriendOneUid", "FriendTwoUid" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Friends_FriendPairFriendOneUid_FriendPairFriend~",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_FriendPairFriendOneUid_FriendPairFriendTwoUid",
                table: "Transaction",
                newName: "IX_Transaction_FriendPairFriendOneUid_FriendPairFriendTwoUid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Friends_FriendPairFriendOneUid_FriendPairFriendT~",
                table: "Transaction",
                columns: new[] { "FriendPairFriendOneUid", "FriendPairFriendTwoUid" },
                principalTable: "Friends",
                principalColumns: new[] { "FriendOneUid", "FriendTwoUid" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
