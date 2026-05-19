using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryPlatoonMVCMain.Data.Migrations
{
    /// <inheritdoc />
    public partial class DonorStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Item",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReceived",
                table: "Donation",
                type: "bit",
                nullable: false,
                defaultValue: false);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "IsReceived",
                table: "Donation");*/
        }
    }
}
