using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportRegister.Server.Migrations
{
    /// <inheritdoc />
    public partial class NewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Owners_OwnerId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owners",
                table: "Owners");

            migrationBuilder.RenameTable(
                name: "Owners",
                newName: "Persons");

            migrationBuilder.RenameColumn(
                name: "Make",
                table: "Vehicles",
                newName: "VIN");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Persons",
                newName: "Signature");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Persons",
                newName: "PersonId");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Vehicles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Constraints",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EngineVolume_CM3",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Horsepower_KW",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Length_CM",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoadCapacity",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficialId",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeatCapacity",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StandingCapacity",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "Vehicles",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Width_CM3",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccountState",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LevelOfAuthority",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Telephone",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BadPoints",
                table: "Persons",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthNumber",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Persons",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Persons",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriversLicenseNumber",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DrivingSuspendedUntil",
                table: "Persons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasSuspendedLicense",
                table: "Persons",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCrimeCommited",
                table: "Persons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficialId",
                table: "Persons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostalCode",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Sex_Male",
                table: "Persons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "PersonId");

            migrationBuilder.CreateTable(
                name: "DriversLicenses",
                columns: table => new
                {
                    DriversLicenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuedOn = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleClass = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriversLicenses", x => x.DriversLicenseId);
                    table.ForeignKey(
                        name: "FK_DriversLicenses_Persons_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicensePlates",
                columns: table => new
                {
                    LicensePlateHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicensePlates", x => x.LicensePlateHistoryId);
                    table.ForeignKey(
                        name: "FK_LicensePlates_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offences",
                columns: table => new
                {
                    OffenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    FineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfficerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OfficialId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offences", x => x.OffenceId);
                    table.ForeignKey(
                        name: "FK_Offences_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offences_Users_OfficerId",
                        column: x => x.OfficerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offences_Users_OfficialId",
                        column: x => x.OfficialId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offences_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Thefts",
                columns: table => new
                {
                    TheftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StolenOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FoundOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportingOfficerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReportingPersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResolvingOfficerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OfficialId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thefts", x => x.TheftId);
                    table.ForeignKey(
                        name: "FK_Thefts_Persons_ReportingPersonId",
                        column: x => x.ReportingPersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Thefts_Users_OfficialId",
                        column: x => x.OfficialId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Thefts_Users_ReportingOfficerId",
                        column: x => x.ReportingOfficerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Thefts_Users_ResolvingOfficerId",
                        column: x => x.ResolvingOfficerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Thefts_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fines",
                columns: table => new
                {
                    FineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PaidOn = table.Column<DateOnly>(type: "date", nullable: false),
                    OffenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fines", x => x.FineId);
                    table.ForeignKey(
                        name: "FK_Fines_Offences_FineId",
                        column: x => x.FineId,
                        principalTable: "Offences",
                        principalColumn: "OffenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OfficialId",
                table: "Vehicles",
                column: "OfficialId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_OfficialId",
                table: "Persons",
                column: "OfficialId");

            migrationBuilder.CreateIndex(
                name: "IX_DriversLicenses_DriverId",
                table: "DriversLicenses",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_LicensePlates_VehicleId",
                table: "LicensePlates",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Offences_OfficerId",
                table: "Offences",
                column: "OfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_Offences_OfficialId",
                table: "Offences",
                column: "OfficialId");

            migrationBuilder.CreateIndex(
                name: "IX_Offences_PersonId",
                table: "Offences",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Offences_VehicleId",
                table: "Offences",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Thefts_OfficialId",
                table: "Thefts",
                column: "OfficialId");

            migrationBuilder.CreateIndex(
                name: "IX_Thefts_ReportingOfficerId",
                table: "Thefts",
                column: "ReportingOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_Thefts_ReportingPersonId",
                table: "Thefts",
                column: "ReportingPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Thefts_ResolvingOfficerId",
                table: "Thefts",
                column: "ResolvingOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_Thefts_VehicleId",
                table: "Thefts",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Users_OfficialId",
                table: "Persons",
                column: "OfficialId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Persons_OwnerId",
                table: "Vehicles",
                column: "OwnerId",
                principalTable: "Persons",
                principalColumn: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Users_OfficialId",
                table: "Vehicles",
                column: "OfficialId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Users_OfficialId",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Persons_OwnerId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Users_OfficialId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "DriversLicenses");

            migrationBuilder.DropTable(
                name: "Fines");

            migrationBuilder.DropTable(
                name: "LicensePlates");

            migrationBuilder.DropTable(
                name: "Thefts");

            migrationBuilder.DropTable(
                name: "Offences");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_OfficialId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_OfficialId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Constraints",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "EngineVolume_CM3",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Horsepower_KW",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Length_CM",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "LoadCapacity",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "OfficialId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "SeatCapacity",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "StandingCapacity",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Width_CM3",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "AccountState",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LevelOfAuthority",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Telephone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "BadPoints",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "BirthNumber",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "DriversLicenseNumber",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "DrivingSuspendedUntil",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "HasSuspendedLicense",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "LastCrimeCommited",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "OfficialId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Sex_Male",
                table: "Persons");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "Owners");

            migrationBuilder.RenameColumn(
                name: "VIN",
                table: "Vehicles",
                newName: "Make");

            migrationBuilder.RenameColumn(
                name: "Signature",
                table: "Owners",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Owners",
                newName: "OwnerId");

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "Vehicles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owners",
                table: "Owners",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Owners_OwnerId",
                table: "Vehicles",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "OwnerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
