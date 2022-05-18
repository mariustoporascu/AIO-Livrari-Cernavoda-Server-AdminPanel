using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class orderETandUseraccept : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstimatedTime",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasUserConfirmedET",
                table: "Orders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "HasUserConfirmedET",
                table: "Orders");
        }
    }
}
