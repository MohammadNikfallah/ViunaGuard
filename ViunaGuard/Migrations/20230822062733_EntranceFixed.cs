using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViunaGuard.Migrations
{
    /// <inheritdoc />
    public partial class EntranceFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_EntranceTypes_EntranceTypeId",
                table: "Entrances");

            migrationBuilder.DropTable(
                name: "EntranceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Entrances_EntranceTypeId",
                table: "Entrances");

            migrationBuilder.DropColumn(
                name: "EntranceTypeId",
                table: "Entrances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntranceTypeId",
                table: "Entrances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EntranceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntranceTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_EntranceTypeId",
                table: "Entrances",
                column: "EntranceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrances_EntranceTypes_EntranceTypeId",
                table: "Entrances",
                column: "EntranceTypeId",
                principalTable: "EntranceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
