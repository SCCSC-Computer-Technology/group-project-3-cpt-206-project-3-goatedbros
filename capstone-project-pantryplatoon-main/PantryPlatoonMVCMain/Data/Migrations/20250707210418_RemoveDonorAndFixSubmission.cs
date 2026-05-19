using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryPlatoonMVCMain.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDonorAndFixSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__DonorSubm__Campu__5DCAEF64",
                table: "DonorSubmission");

            migrationBuilder.DropForeignKey(
                name: "FK__DonorSubm__Donor__5CD6CB2B",
                table: "DonorSubmission");

            migrationBuilder.DropForeignKey(
                name: "FK__DonorSubm__ItemI__619B8048",
                table: "DonorSubmissionItem");

            migrationBuilder.DropForeignKey(
                name: "FK__DonorSubm__Submi__60A75C0F",
                table: "DonorSubmissionItem");

            migrationBuilder.DropTable(
                name: "Donor");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DonorSub__449EE125EC858707",
                table: "DonorSubmission");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "SubmissionId",
                table: "DonorSubmissionItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "DonorSubmissionItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DonorId",
                table: "DonorSubmission",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SCCId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DonorSubmission",
                table: "DonorSubmission",
                column: "SubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DonorSubmission_AspNetUsers_DonorId",
                table: "DonorSubmission",
                column: "DonorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DonorSubmission_Campus_CampusId",
                table: "DonorSubmission",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK__DonorSubm__ItemI__619B8048",
                table: "DonorSubmissionItem",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__DonorSubm__Submi__60A75C0F",
                table: "DonorSubmissionItem",
                column: "SubmissionId",
                principalTable: "DonorSubmission",
                principalColumn: "SubmissionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonorSubmission_AspNetUsers_DonorId",
                table: "DonorSubmission");

            migrationBuilder.DropForeignKey(
                name: "FK_DonorSubmission_Campus_CampusId",
                table: "DonorSubmission");

            migrationBuilder.DropForeignKey(
                name: "FK__DonorSubm__ItemI__619B8048",
                table: "DonorSubmissionItem");

            migrationBuilder.DropForeignKey(
                name: "FK__DonorSubm__Submi__60A75C0F",
                table: "DonorSubmissionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DonorSubmission",
                table: "DonorSubmission");

            migrationBuilder.AlterColumn<int>(
                name: "SubmissionId",
                table: "DonorSubmissionItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "DonorSubmissionItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DonorId",
                table: "DonorSubmission",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "SCCId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__DonorSub__449EE125EC858707",
                table: "DonorSubmission",
                column: "SubmissionId");

            migrationBuilder.CreateTable(
                name: "Donor",
                columns: table => new
                {
                    DonorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Donor__052E3F78B532FC40", x => x.DonorId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK__DonorSubm__Campu__5DCAEF64",
                table: "DonorSubmission",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK__DonorSubm__Donor__5CD6CB2B",
                table: "DonorSubmission",
                column: "DonorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__DonorSubm__ItemI__619B8048",
                table: "DonorSubmissionItem",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK__DonorSubm__Submi__60A75C0F",
                table: "DonorSubmissionItem",
                column: "SubmissionId",
                principalTable: "DonorSubmission",
                principalColumn: "SubmissionId");
        }
    }
}
