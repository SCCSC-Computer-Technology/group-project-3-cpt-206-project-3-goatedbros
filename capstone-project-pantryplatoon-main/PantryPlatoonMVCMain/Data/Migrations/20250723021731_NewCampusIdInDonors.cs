using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryPlatoonMVCMain.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewCampusIdInDonors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Campus_CampusId",
                table: "Donation");

            migrationBuilder.DropTable(
                name: "DonorSubmissionItem");

            migrationBuilder.DropTable(
                name: "DonorSubmission");

            migrationBuilder.AlterColumn<int>(
                name: "CampusId",
                table: "Donation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Campus_CampusId",
                table: "Donation",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Campus_CampusId",
                table: "Donation");

            migrationBuilder.AlterColumn<int>(
                name: "CampusId",
                table: "Donation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DonorSubmission",
                columns: table => new
                {
                    SubmissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampusId = table.Column<int>(type: "int", nullable: true),
                    DonorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmissionDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorSubmission", x => x.SubmissionId);
                    table.ForeignKey(
                        name: "FK_DonorSubmission_AspNetUsers_DonorId",
                        column: x => x.DonorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonorSubmission_Campus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campus",
                        principalColumn: "CampusId");
                });

            migrationBuilder.CreateTable(
                name: "DonorSubmissionItem",
                columns: table => new
                {
                    SubmissionItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    SubmissionId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonorSub__BB8633974DEEFD61", x => x.SubmissionItemId);
                    table.ForeignKey(
                        name: "FK__DonorSubm__ItemI__619B8048",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__DonorSubm__Submi__60A75C0F",
                        column: x => x.SubmissionId,
                        principalTable: "DonorSubmission",
                        principalColumn: "SubmissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DonorSubmission_CampusId",
                table: "DonorSubmission",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_DonorSubmission_DonorId",
                table: "DonorSubmission",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_DonorSubmissionItem_ItemId",
                table: "DonorSubmissionItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DonorSubmissionItem_SubmissionId",
                table: "DonorSubmissionItem",
                column: "SubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Campus_CampusId",
                table: "Donation",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
