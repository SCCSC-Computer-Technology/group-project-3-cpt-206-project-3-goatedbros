using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryPlatoonMVCMain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduledPickupToVisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsScheduledVisit",
                table: "Visit",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledPickupTime",
                table: "Visit",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsScheduledVisit",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "ScheduledPickupTime",
                table: "Visit");
        }
    }
}
