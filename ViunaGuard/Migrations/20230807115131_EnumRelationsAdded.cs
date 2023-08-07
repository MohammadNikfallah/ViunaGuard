using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViunaGuard.Migrations
{
    /// <inheritdoc />
    public partial class EnumRelationsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_People_BirthPlaceCityId",
                table: "People",
                column: "BirthPlaceCityId");

            migrationBuilder.CreateIndex(
                name: "IX_People_CityOfResidenceId",
                table: "People",
                column: "CityOfResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_People_EducationalDegreeId",
                table: "People",
                column: "EducationalDegreeId");

            migrationBuilder.CreateIndex(
                name: "IX_People_GenderId",
                table: "People",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_People_MaritalStatusId",
                table: "People",
                column: "MaritalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_People_MilitaryServiceStatusId",
                table: "People",
                column: "MilitaryServiceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_People_NationalityId",
                table: "People",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_People_ReligionId",
                table: "People",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_EnterOrExitId",
                table: "Entrances",
                column: "EnterOrExitId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_EntranceTypeId",
                table: "Entrances",
                column: "EntranceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeTypeId",
                table: "Employees",
                column: "EmployeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BrandId",
                table: "Cars",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ColorId",
                table: "Cars",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ModelId",
                table: "Cars",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarBrands_BrandId",
                table: "Cars",
                column: "BrandId",
                principalTable: "CarBrands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarModels_ModelId",
                table: "Cars",
                column: "ModelId",
                principalTable: "CarModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Colors_ColorId",
                table: "Cars",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeTypes_EmployeeTypeId",
                table: "Employees",
                column: "EmployeeTypeId",
                principalTable: "EmployeeTypes",
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
                name: "FK_Entrances_EntranceTypes_EntranceTypeId",
                table: "Entrances",
                column: "EntranceTypeId",
                principalTable: "EntranceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_People_Cities_BirthPlaceCityId",
                table: "People",
                column: "BirthPlaceCityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Cities_CityOfResidenceId",
                table: "People",
                column: "CityOfResidenceId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_People_EducationalDegrees_EducationalDegreeId",
                table: "People",
                column: "EducationalDegreeId",
                principalTable: "EducationalDegrees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Genders_GenderId",
                table: "People",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_People_MaritalStatuses_MaritalStatusId",
                table: "People",
                column: "MaritalStatusId",
                principalTable: "MaritalStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_People_MilitaryServiceStatuses_MilitaryServiceStatusId",
                table: "People",
                column: "MilitaryServiceStatusId",
                principalTable: "MilitaryServiceStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Nationalities_NationalityId",
                table: "People",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Religions_ReligionId",
                table: "People",
                column: "ReligionId",
                principalTable: "Religions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarBrands_BrandId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarModels_ModelId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Colors_ColorId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeTypes_EmployeeTypeId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_EnterOrExit_EnterOrExitId",
                table: "Entrances");

            migrationBuilder.DropForeignKey(
                name: "FK_Entrances_EntranceTypes_EntranceTypeId",
                table: "Entrances");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Cities_BirthPlaceCityId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Cities_CityOfResidenceId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_EducationalDegrees_EducationalDegreeId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Genders_GenderId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_MaritalStatuses_MaritalStatusId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_MilitaryServiceStatuses_MilitaryServiceStatusId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Nationalities_NationalityId",
                table: "People");

            migrationBuilder.DropForeignKey(
                name: "FK_People_Religions_ReligionId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_BirthPlaceCityId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_CityOfResidenceId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_EducationalDegreeId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_GenderId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_MaritalStatusId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_MilitaryServiceStatusId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_NationalityId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_People_ReligionId",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_Entrances_EnterOrExitId",
                table: "Entrances");

            migrationBuilder.DropIndex(
                name: "IX_Entrances_EntranceTypeId",
                table: "Entrances");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EmployeeTypeId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Cars_BrandId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_ColorId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_ModelId",
                table: "Cars");
        }
    }
}
