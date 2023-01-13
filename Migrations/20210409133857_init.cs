using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace lab4.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Community",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Budget = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Community", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Advertisement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommunityID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advertisement_Community_CommunityID",
                        column: x => x.CommunityID,
                        principalTable: "Community",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommunityMembership",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    CommunityID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMembership", x => new { x.StudentID, x.CommunityID });
                    table.ForeignKey(
                        name: "FK_CommunityMembership_Community_CommunityID",
                        column: x => x.CommunityID,
                        principalTable: "Community",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityMembership_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentMembership",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    CommunityID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMembership", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StudentMembership_Community_CommunityID",
                        column: x => x.CommunityID,
                        principalTable: "Community",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentMembership_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementCommunity",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementID = table.Column<int>(type: "int", nullable: false),
                    CommunityID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementCommunity", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdvertisementCommunity_Advertisement_AdvertisementID",
                        column: x => x.AdvertisementID,
                        principalTable: "Advertisement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementCommunity_Community_CommunityID",
                        column: x => x.CommunityID,
                        principalTable: "Community",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CommunityID",
                table: "Advertisement",
                column: "CommunityID");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementCommunity_AdvertisementID",
                table: "AdvertisementCommunity",
                column: "AdvertisementID");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementCommunity_CommunityID",
                table: "AdvertisementCommunity",
                column: "CommunityID");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembership_CommunityID",
                table: "CommunityMembership",
                column: "CommunityID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMembership_CommunityID",
                table: "StudentMembership",
                column: "CommunityID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMembership_StudentID",
                table: "StudentMembership",
                column: "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementCommunity");

            migrationBuilder.DropTable(
                name: "CommunityMembership");

            migrationBuilder.DropTable(
                name: "StudentMembership");

            migrationBuilder.DropTable(
                name: "Advertisement");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Community");
        }
    }
}
