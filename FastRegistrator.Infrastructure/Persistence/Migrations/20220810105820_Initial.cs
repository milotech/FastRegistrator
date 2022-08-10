using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastRegistrator.Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountData_Registrations_Id",
                        column: x => x.Id,
                        principalTable: "Registrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Error",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Error", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Error_Registrations_Id",
                        column: x => x.Id,
                        principalTable: "Registrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name_FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passport_Series = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passport_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passport_IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passport_IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Passport_IssueId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passport_Citizenship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Snils = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonData_Registrations_Id",
                        column: x => x.Id,
                        principalTable: "Registrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrizmaChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RejectionReasonCode = table.Column<int>(type: "int", nullable: false),
                    PrizmaResponse = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrizmaChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrizmaChecks_Registrations_Id",
                        column: x => x.Id,
                        principalTable: "Registrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusHistory_Registrations_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "Registrations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistory_RegistrationId",
                table: "StatusHistory",
                column: "RegistrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountData");

            migrationBuilder.DropTable(
                name: "Error");

            migrationBuilder.DropTable(
                name: "PersonData");

            migrationBuilder.DropTable(
                name: "PrizmaChecks");

            migrationBuilder.DropTable(
                name: "StatusHistory");

            migrationBuilder.DropTable(
                name: "Registrations");
        }
    }
}
