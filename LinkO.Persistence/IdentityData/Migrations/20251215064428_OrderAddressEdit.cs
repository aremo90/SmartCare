using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkO.Persistence.IdentityData.Migrations
{
    /// <inheritdoc />
    public partial class OrderAddressEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Address_Street",
                table: "Order",
                newName: "Address_UserAddress");

            migrationBuilder.RenameColumn(
                name: "Address_LastName",
                table: "Order",
                newName: "Address_PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Address_FirstName",
                table: "Order",
                newName: "Address_FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_UserAddress",
                table: "Order",
                newName: "Address_Street");

            migrationBuilder.RenameColumn(
                name: "Address_PhoneNumber",
                table: "Order",
                newName: "Address_LastName");

            migrationBuilder.RenameColumn(
                name: "Address_FullName",
                table: "Order",
                newName: "Address_FirstName");

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
