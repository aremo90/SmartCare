using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCareDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class DevicePair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_UserId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "BatteryLevel",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LastSeenDate",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LastSeenTime",
                table: "Devices");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPaired",
                table: "Devices",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_UserId",
                table: "Devices");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPaired",
                table: "Devices",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "BatteryLevel",
                table: "Devices",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeenDate",
                table: "Devices",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "LastSeenTime",
                table: "Devices",
                type: "time",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId");
        }
    }
}
