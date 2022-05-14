using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class asigndriverorders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverRefId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DriverRefId",
                table: "Orders",
                column: "DriverRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_DriverRefId",
                table: "Orders",
                column: "DriverRefId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_DriverRefId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DriverRefId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DriverRefId",
                table: "Orders");
        }
    }
}
