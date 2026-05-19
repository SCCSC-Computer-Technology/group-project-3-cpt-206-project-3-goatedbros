using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryPlatoonMVCMain.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceUserWithApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__DonorSubm__Donor__5CD6CB2B",
                table: "DonorSubmission");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_AspNetUsers_ApplicationUserId",
                table: "Visit");

            migrationBuilder.DropForeignKey(
                name: "FK__Visits__UserId__59063A47",
                table: "Visit");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Visit_ApplicationUserId",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Visit");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Visit",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DonorId",
                table: "DonorSubmission",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__DonorSubm__Donor__5CD6CB2B",
                table: "DonorSubmission",
                column: "DonorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Visits__UserId__59063A47",
                table: "Visit",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__DonorSubm__Donor__5CD6CB2B",
                table: "DonorSubmission");

            migrationBuilder.DropForeignKey(
                name: "FK__Visits__UserId__59063A47",
                table: "Visit");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Visit",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Visit",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DonorId",
                table: "DonorSubmission",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampusId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SCCId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4C83F34527", x => x.UserId);
                    table.ForeignKey(
                        name: "FK__Users__CampusId__4BAC3F29",
                        column: x => x.CampusId,
                        principalTable: "Campus",
                        principalColumn: "CampusId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visit_ApplicationUserId",
                table: "Visit",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CampusId",
                table: "User",
                column: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK__DonorSubm__Donor__5CD6CB2B",
                table: "DonorSubmission",
                column: "DonorId",
                principalTable: "User",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_AspNetUsers_ApplicationUserId",
                table: "Visit",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Visits__UserId__59063A47",
                table: "Visit",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");
        }
    }
}
