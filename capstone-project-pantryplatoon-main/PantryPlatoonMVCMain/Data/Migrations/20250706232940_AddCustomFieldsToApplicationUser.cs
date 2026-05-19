using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryPlatoonMVCMain.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomFieldsToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "VisitItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "VisitItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Visit",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SCCId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ApplicationUserId",
                table: "Visit",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CampusId",
                table: "AspNetUsers",
                column: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Campus_CampusId",
                table: "AspNetUsers",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_AspNetUsers_ApplicationUserId",
                table: "Visit",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Campus_CampusId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_AspNetUsers_ApplicationUserId",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_Visit_ApplicationUserId",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CampusId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "VisitItem");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "VisitItem");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SCCId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");
        }
    }
}
