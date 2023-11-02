using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace paymatesapi.Migrations
{
    /// <inheritdoc />
    public partial class BankAccount_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_Users_UserUid",
                table: "BankAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccount",
                table: "BankAccount");

            migrationBuilder.RenameTable(
                name: "BankAccount",
                newName: "BankAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccount_UserUid",
                table: "BankAccounts",
                newName: "IX_BankAccounts_UserUid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccounts",
                table: "BankAccounts",
                column: "BankAccountUid");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Users_UserUid",
                table: "BankAccounts",
                column: "UserUid",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Users_UserUid",
                table: "BankAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccounts",
                table: "BankAccounts");

            migrationBuilder.RenameTable(
                name: "BankAccounts",
                newName: "BankAccount");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccounts_UserUid",
                table: "BankAccount",
                newName: "IX_BankAccount_UserUid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccount",
                table: "BankAccount",
                column: "BankAccountUid");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_Users_UserUid",
                table: "BankAccount",
                column: "UserUid",
                principalTable: "Users",
                principalColumn: "Uid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
