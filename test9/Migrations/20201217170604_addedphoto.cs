using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace test9.Migrations
{
    public partial class addedphoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Pnoto",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pnoto",
                table: "AspNetUsers");
        }
    }
}
