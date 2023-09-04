using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViunaGuard.Migrations
{
    /// <inheritdoc />
    public partial class AuthoritiesRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Authorities_AuthorityLevelId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_SignatureNeedForEntrancePermissions_Authorities_MinAuthorityId",
                table: "SignatureNeedForEntrancePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_SignedEntrancePermissions_Authorities_AuthorityID",
                table: "SignedEntrancePermissions");

            migrationBuilder.DropTable(
                name: "Authorities");

            migrationBuilder.DropIndex(
                name: "IX_SignedEntrancePermissions_AuthorityID",
                table: "SignedEntrancePermissions");

            migrationBuilder.DropIndex(
                name: "IX_SignatureNeedForEntrancePermissions_MinAuthorityId",
                table: "SignatureNeedForEntrancePermissions");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AuthorityLevelId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AuthorityLevelId",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "AuthorityID",
                table: "SignedEntrancePermissions",
                newName: "AuthorityLevel");

            migrationBuilder.RenameColumn(
                name: "MinAuthorityId",
                table: "SignatureNeedForEntrancePermissions",
                newName: "MinAuthorityLevel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorityLevel",
                table: "SignedEntrancePermissions",
                newName: "AuthorityID");

            migrationBuilder.RenameColumn(
                name: "MinAuthorityLevel",
                table: "SignatureNeedForEntrancePermissions",
                newName: "MinAuthorityId");

            migrationBuilder.AddColumn<int>(
                name: "AuthorityLevelId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Authorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    AuthorityLevel = table.Column<int>(type: "int", nullable: false),
                    AuthorityLevelName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorities_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignedEntrancePermissions_AuthorityID",
                table: "SignedEntrancePermissions",
                column: "AuthorityID");

            migrationBuilder.CreateIndex(
                name: "IX_SignatureNeedForEntrancePermissions_MinAuthorityId",
                table: "SignatureNeedForEntrancePermissions",
                column: "MinAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AuthorityLevelId",
                table: "Employees",
                column: "AuthorityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorities_OrganizationId",
                table: "Authorities",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Authorities_AuthorityLevelId",
                table: "Employees",
                column: "AuthorityLevelId",
                principalTable: "Authorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignatureNeedForEntrancePermissions_Authorities_MinAuthorityId",
                table: "SignatureNeedForEntrancePermissions",
                column: "MinAuthorityId",
                principalTable: "Authorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignedEntrancePermissions_Authorities_AuthorityID",
                table: "SignedEntrancePermissions",
                column: "AuthorityID",
                principalTable: "Authorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
