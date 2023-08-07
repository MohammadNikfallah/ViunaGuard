using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViunaGuard.Migrations
{
    /// <inheritdoc />
    public partial class SignaturesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntrancePermissions_Employees_PermissionGranterEmployeeId",
                table: "EntrancePermissions");

            migrationBuilder.DropIndex(
                name: "IX_EntrancePermissions_PermissionGranterEmployeeId",
                table: "EntrancePermissions");

            migrationBuilder.DropColumn(
                name: "PermissionGranterEmployeeId",
                table: "EntrancePermissions");

            migrationBuilder.CreateTable(
                name: "SignatureNeedForEntrancePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    AuthorityMinLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignatureNeedForEntrancePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignatureNeedForEntrancePermissions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignedEntrancePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    AuthorityLevel = table.Column<int>(type: "int", nullable: false),
                    SignedByEmployeeId = table.Column<int>(type: "int", nullable: false),
                    EntrancePermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignedEntrancePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignedEntrancePermissions_Employees_SignedByEmployeeId",
                        column: x => x.SignedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SignedEntrancePermissions_EntrancePermissions_EntrancePermissionId",
                        column: x => x.EntrancePermissionId,
                        principalTable: "EntrancePermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignedEntrancePermissions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignatureNeedForEntrancePermissions_OrganizationId",
                table: "SignatureNeedForEntrancePermissions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SignedEntrancePermissions_EntrancePermissionId",
                table: "SignedEntrancePermissions",
                column: "EntrancePermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SignedEntrancePermissions_OrganizationId",
                table: "SignedEntrancePermissions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_SignedEntrancePermissions_SignedByEmployeeId",
                table: "SignedEntrancePermissions",
                column: "SignedByEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignatureNeedForEntrancePermissions");

            migrationBuilder.DropTable(
                name: "SignedEntrancePermissions");

            migrationBuilder.AddColumn<int>(
                name: "PermissionGranterEmployeeId",
                table: "EntrancePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntrancePermissions_PermissionGranterEmployeeId",
                table: "EntrancePermissions",
                column: "PermissionGranterEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntrancePermissions_Employees_PermissionGranterEmployeeId",
                table: "EntrancePermissions",
                column: "PermissionGranterEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
