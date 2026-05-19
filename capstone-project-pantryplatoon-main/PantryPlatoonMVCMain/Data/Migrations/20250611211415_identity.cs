using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryPlatoonMVCMain.Data.Migrations
{
    /// <inheritdoc />
    public partial class identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminReports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalVisits = table.Column<int>(type: "int", nullable: false),
                    TotalStudentsServed = table.Column<int>(type: "int", nullable: false),
                    RepeatVisitors = table.Column<int>(type: "int", nullable: false),
                    TotalItems = table.Column<int>(type: "int", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AdminRep__D5BD48054847CF54", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "Campus",
                columns: table => new
                {
                    CampusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Campuses__FD598DD605EAD8FF", x => x.CampusId);
                });

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

            migrationBuilder.CreateTable(
                name: "ItemCategory",
                columns: table => new
                {
                    ItemCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ItemCate__C24A2925B47D7986", x => x.ItemCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "StaticPages",
                columns: table => new
                {
                    PageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HtmlContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StaticPa__C565B104383F3B0F", x => x.PageId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCCId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CampusId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ItemCategoryId = table.Column<int>(type: "int", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Items__727E838BB912C500", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK__Items__ItemCateg__5070F446",
                        column: x => x.ItemCategoryId,
                        principalTable: "ItemCategory",
                        principalColumn: "ItemCategoryId");
                });

            migrationBuilder.CreateTable(
                name: "DonorSubmission",
                columns: table => new
                {
                    SubmissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorId = table.Column<int>(type: "int", nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: true),
                    SubmissionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonorSub__449EE125EC858707", x => x.SubmissionId);
                    table.ForeignKey(
                        name: "FK__DonorSubm__Campu__5DCAEF64",
                        column: x => x.CampusId,
                        principalTable: "Campus",
                        principalColumn: "CampusId");
                    table.ForeignKey(
                        name: "FK__DonorSubm__Donor__5CD6CB2B",
                        column: x => x.DonorId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Visit",
                columns: table => new
                {
                    VisitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: true),
                    VisitDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Visits__4D3AA1DEE3517811", x => x.VisitId);
                    table.ForeignKey(
                        name: "FK__Visits__CampusId__59FA5E80",
                        column: x => x.CampusId,
                        principalTable: "Campus",
                        principalColumn: "CampusId");
                    table.ForeignKey(
                        name: "FK__Visits__UserId__59063A47",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    CampusId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inventor__F5FDE6B3F5E458C9", x => x.InventoryId);
                    table.ForeignKey(
                        name: "FK__Inventory__Campu__5535A963",
                        column: x => x.CampusId,
                        principalTable: "Campus",
                        principalColumn: "CampusId");
                    table.ForeignKey(
                        name: "FK__Inventory__ItemI__5441852A",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId");
                });

            migrationBuilder.CreateTable(
                name: "DonorSubmissionItem",
                columns: table => new
                {
                    SubmissionItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmissionId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonorSub__BB8633974DEEFD61", x => x.SubmissionItemId);
                    table.ForeignKey(
                        name: "FK__DonorSubm__ItemI__619B8048",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK__DonorSubm__Submi__60A75C0F",
                        column: x => x.SubmissionId,
                        principalTable: "DonorSubmission",
                        principalColumn: "SubmissionId");
                });

            migrationBuilder.CreateTable(
                name: "VisitItem",
                columns: table => new
                {
                    VisitItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    QuantityTaken = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VisitIte__4B99F797008F7E62", x => x.VisitItemId);
                    table.ForeignKey(
                        name: "FK__VisitItem__ItemI__6754599E",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK__VisitItem__Visit__66603565",
                        column: x => x.VisitId,
                        principalTable: "Visit",
                        principalColumn: "VisitId");
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

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_CampusId",
                table: "Inventory",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ItemId",
                table: "Inventory",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemCategoryId",
                table: "Item",
                column: "ItemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CampusId",
                table: "User",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_CampusId",
                table: "Visit",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_UserId",
                table: "Visit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitItem_ItemId",
                table: "VisitItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitItem_VisitId",
                table: "VisitItem",
                column: "VisitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminReports");

            migrationBuilder.DropTable(
                name: "Donor");

            migrationBuilder.DropTable(
                name: "DonorSubmissionItem");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "StaticPages");

            migrationBuilder.DropTable(
                name: "VisitItem");

            migrationBuilder.DropTable(
                name: "DonorSubmission");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Visit");

            migrationBuilder.DropTable(
                name: "ItemCategory");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Campus");
        }
    }
}
