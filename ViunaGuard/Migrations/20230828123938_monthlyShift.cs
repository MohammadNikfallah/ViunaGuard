using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViunaGuard.Migrations
{
    /// <inheritdoc />
    public partial class monthlyShift : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeShiftsPeriodicMonthly",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    DayOfMonth = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuardDoorId = table.Column<int>(type: "int", nullable: true),
                    ShiftMakerEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShiftsPeriodicMonthly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsPeriodicMonthly_Doors_GuardDoorId",
                        column: x => x.GuardDoorId,
                        principalTable: "Doors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsPeriodicMonthly_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsPeriodicMonthly_Employees_ShiftMakerEmployeeId",
                        column: x => x.ShiftMakerEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsPeriodicMonthly_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsPeriodicMonthly_EmployeeId",
                table: "EmployeeShiftsPeriodicMonthly",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsPeriodicMonthly_GuardDoorId",
                table: "EmployeeShiftsPeriodicMonthly",
                column: "GuardDoorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsPeriodicMonthly_OrganizationId",
                table: "EmployeeShiftsPeriodicMonthly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsPeriodicMonthly_ShiftMakerEmployeeId",
                table: "EmployeeShiftsPeriodicMonthly",
                column: "ShiftMakerEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeShiftsPeriodicMonthly");
        }
    }
}
