using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class addedRestSuperMarket3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SuperMarkets_SuperMarketRefId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuperMarkets",
                table: "SuperMarkets");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "SuperMarkets");

            migrationBuilder.AddColumn<int>(
                name: "SuperMarketId",
                table: "SuperMarkets",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuperMarkets",
                table: "SuperMarkets",
                column: "SuperMarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SuperMarkets_SuperMarketRefId",
                table: "Products",
                column: "SuperMarketRefId",
                principalTable: "SuperMarkets",
                principalColumn: "SuperMarketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SuperMarkets_SuperMarketRefId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SuperMarkets",
                table: "SuperMarkets");

            migrationBuilder.DropColumn(
                name: "SuperMarketId",
                table: "SuperMarkets");

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "SuperMarkets",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SuperMarkets",
                table: "SuperMarkets",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SuperMarkets_SuperMarketRefId",
                table: "Products",
                column: "SuperMarketRefId",
                principalTable: "SuperMarkets",
                principalColumn: "RestaurantId");
        }
    }
}
