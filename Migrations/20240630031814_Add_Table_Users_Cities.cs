using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllRiskSolutions_Desafio.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_Users_Cities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_Users_UserId",
                table: "City");

            migrationBuilder.DropPrimaryKey(
                name: "PK_City",
                table: "City");

            migrationBuilder.DropIndex(
                name: "IX_City_UserId",
                table: "City");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "City");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "Cities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CityUser",
                columns: table => new
                {
                    FavoriteCitiesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityUser", x => new { x.FavoriteCitiesId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CityUser_Cities_FavoriteCitiesId",
                        column: x => x.FavoriteCitiesId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CityUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityUser_UserId",
                table: "CityUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "City");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "City",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_City",
                table: "City",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_City_UserId",
                table: "City",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_City_Users_UserId",
                table: "City",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
