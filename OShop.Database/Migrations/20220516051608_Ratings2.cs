using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class Ratings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSecond",
                table: "RatingClients");

            migrationBuilder.AddColumn<bool>(
                name: "FromDriver",
                table: "RatingClients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FromRestaurant",
                table: "RatingClients",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDriver",
                table: "RatingClients");

            migrationBuilder.DropColumn(
                name: "FromRestaurant",
                table: "RatingClients");

            migrationBuilder.AddColumn<bool>(
                name: "IsSecond",
                table: "RatingClients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
