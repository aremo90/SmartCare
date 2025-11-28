using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkO.Persistence.IdentityData.Migrations
{
    /// <inheritdoc />
    public partial class AddressUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingNumber",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Addresses",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Addresses",
                newName: "UserAddress");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Addresses",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "UserAddress",
                table: "Addresses",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Addresses",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Addresses",
                newName: "ZipCode");

            migrationBuilder.AddColumn<int>(
                name: "BuildingNumber",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
