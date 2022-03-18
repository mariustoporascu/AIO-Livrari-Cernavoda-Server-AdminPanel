using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class subcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ShoppingCarts_CartRefId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductRefId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Restaurante_RestaurantRefId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_SuperMarkets_SuperMarketRefId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Orders_OrderRefId",
                table: "ProductInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Products_ProductRefId",
                table: "ProductInOrders");

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryRefId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Photo = table.Column<string>(nullable: true),
                    CategoryRefId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryRefId",
                        column: x => x.CategoryRefId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryRefId",
                table: "SubCategories",
                column: "CategoryRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ShoppingCarts_CartRefId",
                table: "CartItems",
                column: "CartRefId",
                principalTable: "ShoppingCarts",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductRefId",
                table: "CartItems",
                column: "ProductRefId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Restaurante_RestaurantRefId",
                table: "Categories",
                column: "RestaurantRefId",
                principalTable: "Restaurante",
                principalColumn: "RestaurantId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_SuperMarkets_SuperMarketRefId",
                table: "Categories",
                column: "SuperMarketRefId",
                principalTable: "SuperMarkets",
                principalColumn: "SuperMarketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Orders_OrderRefId",
                table: "ProductInOrders",
                column: "OrderRefId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Products_ProductRefId",
                table: "ProductInOrders",
                column: "ProductRefId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ShoppingCarts_CartRefId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductRefId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Restaurante_RestaurantRefId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_SuperMarkets_SuperMarketRefId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Orders_OrderRefId",
                table: "ProductInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Products_ProductRefId",
                table: "ProductInOrders");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropColumn(
                name: "SubCategoryRefId",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ShoppingCarts_CartRefId",
                table: "CartItems",
                column: "CartRefId",
                principalTable: "ShoppingCarts",
                principalColumn: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductRefId",
                table: "CartItems",
                column: "ProductRefId",
                principalTable: "Products",
                principalColumn: "ProductId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Orders_OrderRefId",
                table: "ProductInOrders",
                column: "OrderRefId",
                principalTable: "Orders",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Products_ProductRefId",
                table: "ProductInOrders",
                column: "ProductRefId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
