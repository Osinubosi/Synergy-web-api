using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.Repository.Migrations
{
    public partial class updatecutsomerAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateLogged",
                table: "ResponseLog",
                nullable: false,
                defaultValue: new DateTime(2020, 8, 5, 9, 44, 10, 77, DateTimeKind.Utc).AddTicks(9027),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 7, 27, 10, 19, 28, 898, DateTimeKind.Utc).AddTicks(117));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateLogged",
                table: "RequestLog",
                nullable: false,
                defaultValue: new DateTime(2020, 8, 5, 9, 44, 10, 71, DateTimeKind.Utc).AddTicks(2991),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 7, 27, 10, 19, 28, 892, DateTimeKind.Utc).AddTicks(5615));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "CustomerAccount",
                nullable: false,
                defaultValue: new DateTime(2020, 8, 5, 9, 44, 10, 207, DateTimeKind.Utc).AddTicks(3813),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 7, 27, 10, 19, 29, 30, DateTimeKind.Utc).AddTicks(2759));

            migrationBuilder.AddColumn<string>(
                name: "HowDoyouKnow",
                table: "CustomerAccount",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HowDoyouKnow",
                table: "CustomerAccount");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateLogged",
                table: "ResponseLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 27, 10, 19, 28, 898, DateTimeKind.Utc).AddTicks(117),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 8, 5, 9, 44, 10, 77, DateTimeKind.Utc).AddTicks(9027));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateLogged",
                table: "RequestLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 27, 10, 19, 28, 892, DateTimeKind.Utc).AddTicks(5615),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 8, 5, 9, 44, 10, 71, DateTimeKind.Utc).AddTicks(2991));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "CustomerAccount",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 7, 27, 10, 19, 29, 30, DateTimeKind.Utc).AddTicks(2759),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 8, 5, 9, 44, 10, 207, DateTimeKind.Utc).AddTicks(3813));
        }
    }
}
