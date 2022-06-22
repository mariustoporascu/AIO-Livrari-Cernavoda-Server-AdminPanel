using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OShop.Database.Migrations
{
    public partial class addedcompanyhideinapp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VisibleInApp",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisibleInApp",
                table: "Companies");
        }
    }
}
