using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportRegister.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddOffencePhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OffencePhotos",
                columns: table => new
                {
                    OffencePhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    OffenceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffencePhotos", x => x.OffencePhotoId);
                    table.ForeignKey(
                        name: "FK_OffencePhotos_Offences_OffenceId",
                        column: x => x.OffenceId,
                        principalTable: "Offences",
                        principalColumn: "OffenceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OffencePhotos_OffenceId",
                table: "OffencePhotos",
                column: "OffenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OffencePhotos");
        }
    }
}
