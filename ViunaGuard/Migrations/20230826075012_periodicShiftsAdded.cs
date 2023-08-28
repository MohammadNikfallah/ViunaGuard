using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViunaGuard.Migrations
{
    /// <inheritdoc />
    public partial class periodicShiftsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeShiftsMonthly");

            migrationBuilder.DropTable(
                name: "EmployeeShiftsWeekly");

            migrationBuilder.CreateTable(
                name: "EmployeePeriodicShifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    PeriodDayRange = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuardDoorId = table.Column<int>(type: "int", nullable: true),
                    ShiftMakerEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePeriodicShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePeriodicShifts_Doors_GuardDoorId",
                        column: x => x.GuardDoorId,
                        principalTable: "Doors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeePeriodicShifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeePeriodicShifts_Employees_ShiftMakerEmployeeId",
                        column: x => x.ShiftMakerEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeePeriodicShifts_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePeriodicShifts_EmployeeId",
                table: "EmployeePeriodicShifts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePeriodicShifts_GuardDoorId",
                table: "EmployeePeriodicShifts",
                column: "GuardDoorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePeriodicShifts_OrganizationId",
                table: "EmployeePeriodicShifts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePeriodicShifts_ShiftMakerEmployeeId",
                table: "EmployeePeriodicShifts",
                column: "ShiftMakerEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeePeriodicShifts");

            migrationBuilder.CreateTable(
                name: "EmployeeShiftsMonthly",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    GuardDoorId = table.Column<int>(type: "int", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ShiftMakerEmployeeId = table.Column<int>(type: "int", nullable: true),
                    DayOfMonth = table.Column<int>(type: "int", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShiftsMonthly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsMonthly_Doors_GuardDoorId",
                        column: x => x.GuardDoorId,
                        principalTable: "Doors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsMonthly_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsMonthly_Employees_ShiftMakerEmployeeId",
                        column: x => x.ShiftMakerEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsMonthly_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeShiftsWeekly",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    GuardDoorId = table.Column<int>(type: "int", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ShiftMakerEmployeeId = table.Column<int>(type: "int", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShiftsWeekly", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsWeekly_Doors_GuardDoorId",
                        column: x => x.GuardDoorId,
                        principalTable: "Doors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsWeekly_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsWeekly_Employees_ShiftMakerEmployeeId",
                        column: x => x.ShiftMakerEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeShiftsWeekly_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsMonthly_EmployeeId",
                table: "EmployeeShiftsMonthly",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsMonthly_GuardDoorId",
                table: "EmployeeShiftsMonthly",
                column: "GuardDoorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsMonthly_OrganizationId",
                table: "EmployeeShiftsMonthly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsMonthly_ShiftMakerEmployeeId",
                table: "EmployeeShiftsMonthly",
                column: "ShiftMakerEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsWeekly_EmployeeId",
                table: "EmployeeShiftsWeekly",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsWeekly_GuardDoorId",
                table: "EmployeeShiftsWeekly",
                column: "GuardDoorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsWeekly_OrganizationId",
                table: "EmployeeShiftsWeekly",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftsWeekly_ShiftMakerEmployeeId",
                table: "EmployeeShiftsWeekly",
                column: "ShiftMakerEmployeeId");
        }
    }
}
