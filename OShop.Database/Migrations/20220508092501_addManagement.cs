using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class addManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDriver",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOwner",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantRefId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDriver",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsOwner",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RestaurantRefId",
                table: "AspNetUsers");
        }
    }
}
