using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryPlatoonMVCMain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCampusIdToDonation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Donor",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Donation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Donation_CampusId",
                table: "Donation",
                column: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Campus_CampusId",
                table: "Donation",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Campus_CampusId",
                table: "Donation");

            migrationBuilder.DropIndex(
                name: "IX_Donation_CampusId",
                table: "Donation");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Donation");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Donor",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
