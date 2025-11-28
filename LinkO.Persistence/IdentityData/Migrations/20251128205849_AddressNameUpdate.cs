using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkO.Persistence.IdentityData.Migrations
{
    /// <inheritdoc />
    public partial class AddressNameUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Addresses");
        }
    }
}
