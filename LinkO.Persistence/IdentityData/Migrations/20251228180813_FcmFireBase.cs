using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkO.Persistence.IdentityData.Migrations
{
    /// <inheritdoc />
    public partial class FcmFireBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceFcmToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceFcmToken",
                table: "Users");
        }
    }
}
