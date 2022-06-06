using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OShop.Database.Migrations
{
    public partial class extratipcompanii : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndHour",
                table: "TipCompanies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "TipCompanies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "TipCompanies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartHour",
                table: "TipCompanies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndHour",
                table: "TipCompanies");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "TipCompanies");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "TipCompanies");

            migrationBuilder.DropColumn(
                name: "StartHour",
                table: "TipCompanies");
        }
    }
}
