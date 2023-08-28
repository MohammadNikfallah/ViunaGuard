using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViunaGuard.Migrations
{
    /// <inheritdoc />
    public partial class entranceUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntranceGroups_Cars_CarId",
                table: "EntranceGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_EntranceGroups_People_DriverId",
                table: "EntranceGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_Employees_EmployeeId",
                table: "Entrances");

            migrationBuilder.DropIndex(
                name: "IX_Entrances_EmployeeId",
                table: "Entrances");

            migrationBuilder.DropIndex(
                name: "IX_EntranceGroups_CarId",
                table: "EntranceGroups");

            migrationBuilder.DropIndex(
                name: "IX_EntranceGroups_DriverId",
                table: "EntranceGroups");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Entrances");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "EntranceGroups");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "EntranceGroups");

            migrationBuilder.AddColumn<bool>(
                name: "IsDriver",
                table: "Entrances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeShiftsMonthly");

            migrationBuilder.DropTable(
                name: "EmployeeShiftsWeekly");

            migrationBuilder.DropColumn(
                name: "IsDriver",
                table: "Entrances");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Entrances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "EntranceGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "EntranceGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayOfMonth",
                table: "EmployeeShifts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "EmployeeShifts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "EmployeeShifts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_EmployeeId",
                table: "Entrances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntranceGroups_CarId",
                table: "EntranceGroups",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_EntranceGroups_DriverId",
                table: "EntranceGroups",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntranceGroups_Cars_CarId",
                table: "EntranceGroups",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EntranceGroups_People_DriverId",
                table: "EntranceGroups",
                column: "DriverId",
                principalTable: "People",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrances_Employees_EmployeeId",
                table: "Entrances",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
