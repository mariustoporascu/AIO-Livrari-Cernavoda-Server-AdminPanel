using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class Ratingss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatingClient_Orders_OrderRefId",
                table: "RatingClient");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingClient_AspNetUsers_UserRefId",
                table: "RatingClient");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingDriver_AspNetUsers_DriverRefId",
                table: "RatingDriver");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingDriver_Orders_OrderRefId",
                table: "RatingDriver");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingRestaurant_Orders_OrderRefId",
                table: "RatingRestaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingRestaurant_Restaurante_RestaurantRefId",
                table: "RatingRestaurant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingRestaurant",
                table: "RatingRestaurant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingDriver",
                table: "RatingDriver");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingClient",
                table: "RatingClient");

            migrationBuilder.RenameTable(
                name: "RatingRestaurant",
                newName: "RatingRestaurants");

            migrationBuilder.RenameTable(
                name: "RatingDriver",
                newName: "RatingDrivers");

            migrationBuilder.RenameTable(
                name: "RatingClient",
                newName: "RatingClients");

            migrationBuilder.RenameIndex(
                name: "IX_RatingRestaurant_RestaurantRefId",
                table: "RatingRestaurants",
                newName: "IX_RatingRestaurants_RestaurantRefId");

            migrationBuilder.RenameIndex(
                name: "IX_RatingDriver_DriverRefId",
                table: "RatingDrivers",
                newName: "IX_RatingDrivers_DriverRefId");

            migrationBuilder.RenameIndex(
                name: "IX_RatingClient_UserRefId",
                table: "RatingClients",
                newName: "IX_RatingClients_UserRefId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingRestaurants",
                table: "RatingRestaurants",
                columns: new[] { "OrderRefId", "RestaurantRefId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingDrivers",
                table: "RatingDrivers",
                columns: new[] { "OrderRefId", "DriverRefId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingClients",
                table: "RatingClients",
                columns: new[] { "OrderRefId", "UserRefId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RatingClients_Orders_OrderRefId",
                table: "RatingClients",
                column: "OrderRefId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingClients_AspNetUsers_UserRefId",
                table: "RatingClients",
                column: "UserRefId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingDrivers_AspNetUsers_DriverRefId",
                table: "RatingDrivers",
                column: "DriverRefId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingDrivers_Orders_OrderRefId",
                table: "RatingDrivers",
                column: "OrderRefId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingRestaurants_Orders_OrderRefId",
                table: "RatingRestaurants",
                column: "OrderRefId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingRestaurants_Restaurante_RestaurantRefId",
                table: "RatingRestaurants",
                column: "RestaurantRefId",
                principalTable: "Restaurante",
                principalColumn: "RestaurantId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatingClients_Orders_OrderRefId",
                table: "RatingClients");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingClients_AspNetUsers_UserRefId",
                table: "RatingClients");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingDrivers_AspNetUsers_DriverRefId",
                table: "RatingDrivers");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingDrivers_Orders_OrderRefId",
                table: "RatingDrivers");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingRestaurants_Orders_OrderRefId",
                table: "RatingRestaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_RatingRestaurants_Restaurante_RestaurantRefId",
                table: "RatingRestaurants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingRestaurants",
                table: "RatingRestaurants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingDrivers",
                table: "RatingDrivers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RatingClients",
                table: "RatingClients");

            migrationBuilder.RenameTable(
                name: "RatingRestaurants",
                newName: "RatingRestaurant");

            migrationBuilder.RenameTable(
                name: "RatingDrivers",
                newName: "RatingDriver");

            migrationBuilder.RenameTable(
                name: "RatingClients",
                newName: "RatingClient");

            migrationBuilder.RenameIndex(
                name: "IX_RatingRestaurants_RestaurantRefId",
                table: "RatingRestaurant",
                newName: "IX_RatingRestaurant_RestaurantRefId");

            migrationBuilder.RenameIndex(
                name: "IX_RatingDrivers_DriverRefId",
                table: "RatingDriver",
                newName: "IX_RatingDriver_DriverRefId");

            migrationBuilder.RenameIndex(
                name: "IX_RatingClients_UserRefId",
                table: "RatingClient",
                newName: "IX_RatingClient_UserRefId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingRestaurant",
                table: "RatingRestaurant",
                columns: new[] { "OrderRefId", "RestaurantRefId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingDriver",
                table: "RatingDriver",
                columns: new[] { "OrderRefId", "DriverRefId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RatingClient",
                table: "RatingClient",
                columns: new[] { "OrderRefId", "UserRefId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RatingClient_Orders_OrderRefId",
                table: "RatingClient",
                column: "OrderRefId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingClient_AspNetUsers_UserRefId",
                table: "RatingClient",
                column: "UserRefId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingDriver_AspNetUsers_DriverRefId",
                table: "RatingDriver",
                column: "DriverRefId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingDriver_Orders_OrderRefId",
                table: "RatingDriver",
                column: "OrderRefId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingRestaurant_Orders_OrderRefId",
                table: "RatingRestaurant",
                column: "OrderRefId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RatingRestaurant_Restaurante_RestaurantRefId",
                table: "RatingRestaurant",
                column: "RestaurantRefId",
                principalTable: "Restaurante",
                principalColumn: "RestaurantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
