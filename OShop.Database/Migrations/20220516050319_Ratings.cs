using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class Ratings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RatingClient",
                columns: table => new
                {
                    UserRefId = table.Column<string>(nullable: false),
                    OrderRefId = table.Column<int>(nullable: false),
                    IsSecond = table.Column<bool>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingClient", x => new { x.OrderRefId, x.UserRefId });
                    table.ForeignKey(
                        name: "FK_RatingClient_Orders_OrderRefId",
                        column: x => x.OrderRefId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RatingClient_AspNetUsers_UserRefId",
                        column: x => x.UserRefId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RatingDriver",
                columns: table => new
                {
                    DriverRefId = table.Column<string>(nullable: false),
                    OrderRefId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingDriver", x => new { x.OrderRefId, x.DriverRefId });
                    table.ForeignKey(
                        name: "FK_RatingDriver_AspNetUsers_DriverRefId",
                        column: x => x.DriverRefId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RatingDriver_Orders_OrderRefId",
                        column: x => x.OrderRefId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RatingRestaurant",
                columns: table => new
                {
                    RestaurantRefId = table.Column<int>(nullable: false),
                    OrderRefId = table.Column<int>(nullable: false),
                    Rating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingRestaurant", x => new { x.OrderRefId, x.RestaurantRefId });
                    table.ForeignKey(
                        name: "FK_RatingRestaurant_Orders_OrderRefId",
                        column: x => x.OrderRefId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RatingRestaurant_Restaurante_RestaurantRefId",
                        column: x => x.RestaurantRefId,
                        principalTable: "Restaurante",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RatingClient_UserRefId",
                table: "RatingClient",
                column: "UserRefId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingDriver_DriverRefId",
                table: "RatingDriver",
                column: "DriverRefId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingRestaurant_RestaurantRefId",
                table: "RatingRestaurant",
                column: "RestaurantRefId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RatingClient");

            migrationBuilder.DropTable(
                name: "RatingDriver");

            migrationBuilder.DropTable(
                name: "RatingRestaurant");
        }
    }
}
