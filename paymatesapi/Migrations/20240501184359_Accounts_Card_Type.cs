using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace paymatesapi.Migrations
{
    /// <inheritdoc />
    public partial class Accounts_Card_Type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "BankAccounts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardType",
                table: "BankAccounts");
        }
    }
}
