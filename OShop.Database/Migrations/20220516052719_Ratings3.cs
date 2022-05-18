using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class Ratings3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDriver",
                table: "RatingClients");

            migrationBuilder.DropColumn(
                name: "FromRestaurant",
                table: "RatingClients");

            migrationBuilder.AddColumn<bool>(
                name: "ClientGaveRatingDriver",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ClientGaveRatingRestaurant",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DriverGaveRating",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RestaurantGaveRating",
                table: "Orders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientGaveRatingDriver",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ClientGaveRatingRestaurant",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DriverGaveRating",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RestaurantGaveRating",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "FromDriver",
                table: "RatingClients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FromRestaurant",
                table: "RatingClients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
