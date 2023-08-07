using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViunaGuard.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    ModelId = table.Column<int>(type: "int", nullable: true),
                    VIN = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntrancePolicies",
                columns: table => new
                {
                    DoorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckCars = table.Column<bool>(type: "bit", nullable: false),
                    CheckPoeple = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrancePolicies", x => x.DoorId);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationPolicies",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckGuests = table.Column<bool>(type: "bit", nullable: false),
                    CheckUnregisteredGuests = table.Column<bool>(type: "bit", nullable: false),
                    CheckCarsOnConferenceMode = table.Column<bool>(type: "bit", nullable: false),
                    CheckpeopleOnConferenceMode = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationPolicies", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonAdditionalInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonAdditionalInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CanChangeShifts = table.Column<bool>(type: "bit", nullable: false),
                    CanBringGuests = table.Column<bool>(type: "bit", nullable: false),
                    CanInviteGuests = table.Column<bool>(type: "bit", nullable: false),
                    AlwaysHaveEntrancePermission = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authorities",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    AuthorityLevel = table.Column<int>(type: "int", nullable: false),
                    AuthorityLevelName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorities", x => x.OrganizationId);
                    table.ForeignKey(
                        name: "FK_Authorities_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doors_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonAdditionalInfoId = table.Column<int>(type: "int", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CellPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FathersName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    BirthPlaceId = table.Column<int>(type: "int", nullable: true),
                    BirthDay = table.Column<DateTime>(type: "date", nullable: true),
                    CityOfResidenceId = table.Column<int>(type: "int", nullable: true),
                    EducationalDegreeId = table.Column<int>(type: "int", nullable: true),
                    MilitaryServiceStatsCode = table.Column<int>(type: "int", nullable: true),
                    NationalityId = table.Column<int>(type: "int", nullable: true),
                    ReligionId = table.Column<int>(type: "int", nullable: true),
                    MaritalStatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_PersonAdditionalInfos_PersonAdditionalInfoId",
                        column: x => x.PersonAdditionalInfoId,
                        principalTable: "PersonAdditionalInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlackList",
                columns: table => new
                {
                    AreBannedId = table.Column<int>(type: "int", nullable: false),
                    BannedFromId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackList", x => new { x.AreBannedId, x.BannedFromId });
                    table.ForeignKey(
                        name: "FK_BlackList_Organizations_BannedFromId",
                        column: x => x.BannedFromId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlackList_People_AreBannedId",
                        column: x => x.AreBannedId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarPerson",
                columns: table => new
                {
                    CarsId = table.Column<int>(type: "int", nullable: false),
                    PeopleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarPerson", x => new { x.CarsId, x.PeopleId });
                    table.ForeignKey(
                        name: "FK_CarPerson_Cars_CarsId",
                        column: x => x.CarsId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarPerson_People_PeopleId",
                        column: x => x.PeopleId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    EmployeeTypeId = table.Column<int>(type: "int", nullable: false),
                    PersonnelID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorityLevelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Authorities_AuthorityLevelId",
                        column: x => x.AuthorityLevelId,
                        principalTable: "Authorities",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Employees_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntranceGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    CarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntranceGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntranceGroups_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntranceGroups_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntranceGroups_People_DriverId",
                        column: x => x.DriverId,
                        principalTable: "People",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeShifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuardDoorId = table.Column<int>(type: "int", nullable: true),
                    ShiftMakerEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Doors_GuardDoorId",
                        column: x => x.GuardDoorId,
                        principalTable: "Doors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Employees_ShiftMakerEmployeeId",
                        column: x => x.ShiftMakerEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeShiftsMonthly",
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
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeShiftsWeekly",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuardDoorId = table.Column<int>(type: "int", nullable: true),
                    ShiftMakerEmployeeId = table.Column<int>(type: "int", nullable: true)
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
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EntrancePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    StartValidityTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndValidityTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: true),
                    PermissionGranted = table.Column<bool>(type: "bit", nullable: false),
                    PermissionGranterEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrancePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntrancePermissions_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntrancePermissions_Employees_PermissionGranterEmployeeId",
                        column: x => x.PermissionGranterEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EntrancePermissions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntrancePermissions_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Entrances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: true),
                    GuestCount = table.Column<int>(type: "int", nullable: true),
                    GuardId = table.Column<int>(type: "int", nullable: false),
                    DoorId = table.Column<int>(type: "int", nullable: false),
                    EntranceGroupId = table.Column<int>(type: "int", nullable: true),
                    EntranceTypeId = table.Column<int>(type: "int", nullable: false),
                    EnterOrExitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entrances_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entrances_Doors_DoorId",
                        column: x => x.DoorId,
                        principalTable: "Doors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entrances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entrances_Employees_GuardId",
                        column: x => x.GuardId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Entrances_EntranceGroups_EntranceGroupId",
                        column: x => x.EntranceGroupId,
                        principalTable: "EntranceGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entrances_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Entrances_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlackList_BannedFromId",
                table: "BlackList",
                column: "BannedFromId");

            migrationBuilder.CreateIndex(
                name: "IX_CarPerson_PeopleId",
                table: "CarPerson",
                column: "PeopleId");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_OrganizationId",
                table: "Doors",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AuthorityLevelId",
                table: "Employees",
                column: "AuthorityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_OrganizationId",
                table: "Employees",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PersonId",
                table: "Employees",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_EmployeeId",
                table: "EmployeeShifts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_GuardDoorId",
                table: "EmployeeShifts",
                column: "GuardDoorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_OrganizationId",
                table: "EmployeeShifts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_ShiftMakerEmployeeId",
                table: "EmployeeShifts",
                column: "ShiftMakerEmployeeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_EntranceGroups_CarId",
                table: "EntranceGroups",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_EntranceGroups_DriverId",
                table: "EntranceGroups",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_EntranceGroups_OrganizationId",
                table: "EntranceGroups",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrancePermissions_CarId",
                table: "EntrancePermissions",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrancePermissions_OrganizationId",
                table: "EntrancePermissions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrancePermissions_PermissionGranterEmployeeId",
                table: "EntrancePermissions",
                column: "PermissionGranterEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrancePermissions_PersonId",
                table: "EntrancePermissions",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_CarId",
                table: "Entrances",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_DoorId",
                table: "Entrances",
                column: "DoorId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_EmployeeId",
                table: "Entrances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_EntranceGroupId",
                table: "Entrances",
                column: "EntranceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_GuardId",
                table: "Entrances",
                column: "GuardId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_OrganizationId",
                table: "Entrances",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrances_PersonId",
                table: "Entrances",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_People_PersonAdditionalInfoId",
                table: "People",
                column: "PersonAdditionalInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackList");

            migrationBuilder.DropTable(
                name: "CarPerson");

            migrationBuilder.DropTable(
                name: "EmployeeShifts");

            migrationBuilder.DropTable(
                name: "EmployeeShiftsMonthly");

            migrationBuilder.DropTable(
                name: "EmployeeShiftsWeekly");

            migrationBuilder.DropTable(
                name: "EntrancePermissions");

            migrationBuilder.DropTable(
                name: "EntrancePolicies");

            migrationBuilder.DropTable(
                name: "Entrances");

            migrationBuilder.DropTable(
                name: "OrganizationPolicies");

            migrationBuilder.DropTable(
                name: "UserAccesses");

            migrationBuilder.DropTable(
                name: "Doors");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "EntranceGroups");

            migrationBuilder.DropTable(
                name: "Authorities");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "PersonAdditionalInfos");
        }
    }
}
