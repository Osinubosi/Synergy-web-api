using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.Repository.Migrations
{
    public partial class Createinitailizationmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(maxLength: 100, nullable: false),
                    CountryShortCode = table.Column<string>(maxLength: 5, nullable: false),
                    DailingCode = table.Column<string>(maxLength: 5, nullable: true),
                    CountryImageName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(nullable: false),
                    Payload = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: false),
                    DateLogged = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 7, 27, 10, 19, 28, 892, DateTimeKind.Utc).AddTicks(5615)),
                    RequestrefernceId = table.Column<string>(nullable: true),
                    UniqueReferenceId = table.Column<string>(nullable: false),
                    RequestMethod = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResponseLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueReferenceId = table.Column<string>(nullable: false),
                    Payload = table.Column<string>(nullable: false),
                    DateLogged = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 7, 27, 10, 19, 28, 898, DateTimeKind.Utc).AddTicks(117)),
                    StatusCode = table.Column<int>(nullable: false),
                    SynergyStatusCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAccount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    DailingCode = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    PasswordKey = table.Column<string>(nullable: false),
                    isEmailVerified = table.Column<bool>(nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 7, 27, 10, 19, 29, 30, DateTimeKind.Utc).AddTicks(2759)),
                    RefererCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAccount_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_phoneNumbers_UniqueIndex",
                table: "Country",
                columns: new[] { "DailingCode", "CountryName", "CountryShortCode" },
                unique: true,
                filter: "[DailingCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccount_CountryId",
                table: "CustomerAccount",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAddress_UniqueIndex",
                table: "CustomerAccount",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_phoneNumbers_UniqueIndex",
                table: "CustomerAccount",
                column: "PhoneNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerAccount");

            migrationBuilder.DropTable(
                name: "RequestLog");

            migrationBuilder.DropTable(
                name: "ResponseLog");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
