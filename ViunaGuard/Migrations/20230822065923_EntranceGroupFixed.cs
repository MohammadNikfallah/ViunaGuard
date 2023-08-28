using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViunaGuard.Migrations
{
    /// <inheritdoc />
    public partial class EntranceGroupFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_Doors_DoorId",
                table: "Entrances");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_Employees_GuardId",
                table: "Entrances");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_EnterOrExit_EnterOrExitId",
                table: "Entrances");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_EntranceGroups_EntranceGroupId",
                table: "Entrances");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_Organizations_OrganizationId",
                table: "Entrances");

            migrationBuilder.DropIndex(
                name: "IX_Entrances_DoorId",
                table: "Entrances");

            migrationBuilder.DropIndex(
                name: "IX_Entrances_EnterOrExitId",
                table: "Entrances");

            migrationBuilder.DropIndex(
                name: "IX_Entrances_GuardId",
                table: "Entrances");

            migrationBuilder.DropColumn(
                name: "DoorId",
                table: "Entrances");

            migrationBuilder.DropColumn(
                name: "EnterOrExitId",
                table: "Entrances");

            migrationBuilder.DropColumn(
                name: "GuardId",
                table: "Entrances");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Entrances");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "Entrances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EntranceGroupId",
                table: "Entrances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoorId",
                table: "EntranceGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnterOrExitId",
                table: "EntranceGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuardId",
                table: "EntranceGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EntranceGroups_DoorId",
                table: "EntranceGroups",
                column: "DoorId");

            migrationBuilder.CreateIndex(
                name: "IX_EntranceGroups_EnterOrExitId",
                table: "EntranceGroups",
                column: "EnterOrExitId");

            migrationBuilder.CreateIndex(
                name: "IX_EntranceGroups_GuardId",
                table: "EntranceGroups",
                column: "GuardId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntranceGroups_Doors_DoorId",
                table: "EntranceGroups",
                column: "DoorId",
                principalTable: "Doors",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_EntranceGroups_Employees_GuardId",
                table: "EntranceGroups",
                column: "GuardId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_EntranceGroups_EnterOrExit_EnterOrExitId",
                table: "EntranceGroups",
                column: "EnterOrExitId",
                principalTable: "EnterOrExit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrances_EntranceGroups_EntranceGroupId",
                table: "Entrances",
                column: "EntranceGroupId",
                principalTable: "EntranceGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrances_Organizations_OrganizationId",
                table: "Entrances",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntranceGroups_Doors_DoorId",
                table: "EntranceGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_EntranceGroups_Employees_GuardId",
                table: "EntranceGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_EntranceGroups_EnterOrExit_EnterOrExitId",
                table: "EntranceGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_EntranceGroups_EntranceGroupId",
                table: "Entrances");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_Organizations_OrganizationId",
                table: "Entrances");

            migrationBuilder.DropIndex(
                name: "IX_EntranceGroups_DoorId",
                table: "EntranceGroups");

            migrationBuilder.DropIndex(
                name: "IX_EntranceGroups_EnterOrExitId",
                table: "EntranceGroups");

            migrationBuilder.DropIndex(
                name: "IX_EntranceGroups_GuardId",
                table: "EntranceGroups");

            migrationBuilder.DropColumn(
                name: "DoorId",
                table: "EntranceGroups");

            migrationBuilder.DropColumn(
                name: "EnterOrExitId",
                table: "EntranceGroups");

            migrationBuilder.DropColumn(
                name: "GuardId",
                table: "EntranceGroups");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationId",
                table: "Entrances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EntranceGroupId",
                table: "Entrances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "DoorId",
                table: "Entrances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnterOrExitId",
                table: "Entrances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuardId",
                table: "Entrances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "Entrances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_DoorId",
                table: "Entrances",
                column: "DoorId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_EnterOrExitId",
                table: "Entrances",
                column: "EnterOrExitId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_GuardId",
                table: "Entrances",
                column: "GuardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrances_Doors_DoorId",
                table: "Entrances",
                column: "DoorId",
                principalTable: "Doors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrances_Employees_GuardId",
                table: "Entrances",
                column: "GuardId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrances_EnterOrExit_EnterOrExitId",
                table: "Entrances",
                column: "EnterOrExitId",
                principalTable: "EnterOrExit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entrances_EntranceGroups_EntranceGroupId",
                table: "Entrances",
                column: "EntranceGroupId",
                principalTable: "EntranceGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrances_Organizations_OrganizationId",
                table: "Entrances",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
