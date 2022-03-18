using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class addedRestSuperMarket2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantRefId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuperMarketRefId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Restaurante",
                columns: table => new
                {
                    RestaurantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Photo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurante", x => x.RestaurantId);
                });

            migrationBuilder.CreateTable(
                name: "SuperMarkets",
                columns: table => new
                {
                    RestaurantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Photo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperMarkets", x => x.RestaurantId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_RestaurantRefId",
                table: "Products",
                column: "RestaurantRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SuperMarketRefId",
                table: "Products",
                column: "SuperMarketRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Restaurante_RestaurantRefId",
                table: "Products",
                column: "RestaurantRefId",
                principalTable: "Restaurante",
                principalColumn: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SuperMarkets_SuperMarketRefId",
                table: "Products",
                column: "SuperMarketRefId",
                principalTable: "SuperMarkets",
                principalColumn: "RestaurantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Restaurante_RestaurantRefId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SuperMarkets_SuperMarketRefId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Restaurante");

            migrationBuilder.DropTable(
                name: "SuperMarkets");

            migrationBuilder.DropIndex(
                name: "IX_Products_RestaurantRefId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SuperMarketRefId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RestaurantRefId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SuperMarketRefId",
                table: "Products");
        }
    }
}
