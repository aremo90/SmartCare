using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkO.Persistence.IdentityData.Migrations
{
    /// <inheritdoc />
    public partial class MedicineUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MedicineReminders_UserId",
                table: "MedicineReminders");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineReminders_UserId",
                table: "MedicineReminders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MedicineReminders_UserId",
                table: "MedicineReminders");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineReminders_UserId",
                table: "MedicineReminders",
                column: "UserId",
                unique: true);
        }
    }
}
