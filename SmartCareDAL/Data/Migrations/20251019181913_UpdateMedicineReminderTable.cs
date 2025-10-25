using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCareDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicineReminderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderTime",
                table: "MedicineReminders");

            migrationBuilder.AddColumn<string>(
                name: "DaysOfWeek",
                table: "MedicineReminders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepeatPattern",
                table: "MedicineReminders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleDate",
                table: "MedicineReminders",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ScheduleTime",
                table: "MedicineReminders",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DeviceIdentifier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastSeenDate = table.Column<DateTime>(type: "date", nullable: true),
                    LastSeenTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsPaired = table.Column<bool>(type: "bit", nullable: false),
                    BatteryLevel = table.Column<double>(type: "float", nullable: true),
                    SignalStrength = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropColumn(
                name: "DaysOfWeek",
                table: "MedicineReminders");

            migrationBuilder.DropColumn(
                name: "RepeatPattern",
                table: "MedicineReminders");

            migrationBuilder.DropColumn(
                name: "ScheduleDate",
                table: "MedicineReminders");

            migrationBuilder.DropColumn(
                name: "ScheduleTime",
                table: "MedicineReminders");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReminderTime",
                table: "MedicineReminders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
