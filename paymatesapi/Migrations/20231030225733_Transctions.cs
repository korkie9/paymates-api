using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace paymatesapi.Migrations
{
    /// <inheritdoc />
    public partial class Transctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Uid = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DebtorUid = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreditorUid = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FriendPairFriendOneUid = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FriendPairFriendTwoUid = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_Transaction_Friends_FriendPairFriendOneUid_FriendPairFriendT~",
                        columns: x => new { x.FriendPairFriendOneUid, x.FriendPairFriendTwoUid },
                        principalTable: "Friends",
                        principalColumns: new[] { "FriendOneUid", "FriendTwoUid" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FriendPairFriendOneUid_FriendPairFriendTwoUid",
                table: "Transaction",
                columns: new[] { "FriendPairFriendOneUid", "FriendPairFriendTwoUid" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}
