using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllRiskSolutions_Desafio.Migrations
{
    /// <inheritdoc />
    public partial class Record_To_Primitive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Coords_Longitude",
                table: "Cities",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Coords_Latitude",
                table: "Cities",
                newName: "Latitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Cities",
                newName: "Coords_Longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Cities",
                newName: "Coords_Latitude");
        }
    }
}
