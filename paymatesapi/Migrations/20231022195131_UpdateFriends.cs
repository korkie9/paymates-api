using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace paymatesapi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFriends : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FriendUid",
                table: "Friends",
                newName: "FriendTwoUid");

            migrationBuilder.RenameColumn(
                name: "Uid",
                table: "Friends",
                newName: "FriendOneUid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FriendTwoUid",
                table: "Friends",
                newName: "FriendUid");

            migrationBuilder.RenameColumn(
                name: "FriendOneUid",
                table: "Friends",
                newName: "Uid");
        }
    }
}
