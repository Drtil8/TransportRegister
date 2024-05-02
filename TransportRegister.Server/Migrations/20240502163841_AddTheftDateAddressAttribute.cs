using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportRegister.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddTheftDateAddressAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Thefts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Thefts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_HouseNumber",
                table: "Thefts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_PostalCode",
                table: "Thefts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                table: "Thefts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                table: "Thefts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedOn",
                table: "Thefts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Thefts");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Thefts");

            migrationBuilder.DropColumn(
                name: "Address_HouseNumber",
                table: "Thefts");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                table: "Thefts");

            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "Thefts");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                table: "Thefts");

            migrationBuilder.DropColumn(
                name: "ReturnedOn",
                table: "Thefts");
        }
    }
}
