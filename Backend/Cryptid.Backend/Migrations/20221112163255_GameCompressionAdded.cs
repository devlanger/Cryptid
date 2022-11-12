using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptid.Backend.Migrations
{
    public partial class GameCompressionAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CurrentState",
                table: "Games",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "Games");
        }
    }
}
