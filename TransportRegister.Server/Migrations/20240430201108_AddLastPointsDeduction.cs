using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportRegister.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddLastPointsDeduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPointsDeduction",
                table: "Persons",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPointsDeduction",
                table: "Persons");
        }
    }
}
