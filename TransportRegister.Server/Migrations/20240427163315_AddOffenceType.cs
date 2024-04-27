using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportRegister.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddOffenceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OffenceTypeId",
                table: "Offences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OffenceTypes",
                columns: table => new
                {
                    OffenceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffenceTypes", x => x.OffenceTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offences_OffenceTypeId",
                table: "Offences",
                column: "OffenceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offences_OffenceTypes_OffenceTypeId",
                table: "Offences",
                column: "OffenceTypeId",
                principalTable: "OffenceTypes",
                principalColumn: "OffenceTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offences_OffenceTypes_OffenceTypeId",
                table: "Offences");

            migrationBuilder.DropTable(
                name: "OffenceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Offences_OffenceTypeId",
                table: "Offences");

            migrationBuilder.DropColumn(
                name: "OffenceTypeId",
                table: "Offences");
        }
    }
}
