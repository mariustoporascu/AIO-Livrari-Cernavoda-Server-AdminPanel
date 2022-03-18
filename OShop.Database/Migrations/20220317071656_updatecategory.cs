using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class updatecategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantRefId",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuperMarketRefId",
                table: "Categories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_RestaurantRefId",
                table: "Categories",
                column: "RestaurantRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SuperMarketRefId",
                table: "Categories",
                column: "SuperMarketRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Restaurante_RestaurantRefId",
                table: "Categories",
                column: "RestaurantRefId",
                principalTable: "Restaurante",
                principalColumn: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_SuperMarkets_SuperMarketRefId",
                table: "Categories",
                column: "SuperMarketRefId",
                principalTable: "SuperMarkets",
                principalColumn: "SuperMarketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Restaurante_RestaurantRefId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_SuperMarkets_SuperMarketRefId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_RestaurantRefId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SuperMarketRefId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "RestaurantRefId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SuperMarketRefId",
                table: "Categories");
        }
    }
}
