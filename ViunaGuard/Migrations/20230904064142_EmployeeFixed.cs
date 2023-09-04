using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViunaGuard.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserAccessRoleId",
                table: "Employees",
                type: "int",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserAccessRoleId",
                table: "Employees",
                column: "UserAccessRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_UserAccessRole_UserAccessRoleId",
                table: "Employees",
                column: "UserAccessRoleId",
                principalTable: "UserAccessRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_UserAccessRole_UserAccessRoleId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserAccessRoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserAccessRoleId",
                table: "Employees");
        }
    }
}
