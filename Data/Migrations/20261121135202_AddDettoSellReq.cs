using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDettoSellReq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "SellerRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "SellerRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "SellerRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "SellerRequests");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "SellerRequests");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "SellerRequests");
        }
    }
}
