using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class Ratings4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "RatingClients");

            migrationBuilder.AddColumn<int>(
                name: "RatingDeLaRestaurant",
                table: "RatingClients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RatingDeLaSofer",
                table: "RatingClients",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingDeLaRestaurant",
                table: "RatingClients");

            migrationBuilder.DropColumn(
                name: "RatingDeLaSofer",
                table: "RatingClients");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "RatingClients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
