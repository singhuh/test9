using Microsoft.EntityFrameworkCore.Migrations;

namespace test9.Migrations
{
    public partial class changepnototophoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pnoto",
                table: "AspNetUsers",
                newName: "Photo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "AspNetUsers",
                newName: "Pnoto");
        }
    }
}
