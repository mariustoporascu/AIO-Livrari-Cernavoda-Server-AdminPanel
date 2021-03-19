using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class update1 : Migration
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
                name: "FK_ProductInOrders_Orders_OrderRefId",
                table: "ProductInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Products_ProductRefId",
                table: "ProductInOrders");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ShoppingCarts_CartRefId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductRefId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Orders_OrderRefId",
                table: "ProductInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Products_ProductRefId",
                table: "ProductInOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ShoppingCarts_CartRefId",
                table: "CartItems",
                column: "CartRefId",
                principalTable: "ShoppingCarts",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductRefId",
                table: "CartItems",
                column: "ProductRefId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Orders_OrderRefId",
                table: "ProductInOrders",
                column: "OrderRefId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Products_ProductRefId",
                table: "ProductInOrders",
                column: "ProductRefId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
