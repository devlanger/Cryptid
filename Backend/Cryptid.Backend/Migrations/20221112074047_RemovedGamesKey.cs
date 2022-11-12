using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptid.Backend.Migrations
{
    public partial class RemovedGamesKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Games_Id",
                table: "Players");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Players_Games_Id",
                table: "Players",
                column: "Id",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
