using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryPlatoonMVCMain.Migrations
{
    /// <inheritdoc />
    public partial class AddDemographicFieldsToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalNotes",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdultsInHousehold",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BenefitsStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Children5To18",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChildrenUnder5",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DietaryRestrictionExplanation",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasDietaryRestrictions",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HouseholdEmploymentStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KitchenAccess",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialtyItemsExplanation",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialtyItemsNeeded",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalNotes",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdultsInHousehold",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BenefitsStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Children5To18",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChildrenUnder5",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DietaryRestrictionExplanation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmploymentStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HasDietaryRestrictions",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HouseholdEmploymentStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "KitchenAccess",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpecialtyItemsExplanation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpecialtyItemsNeeded",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentStatus",
                table: "AspNetUsers");
        }
    }
}
