using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class addedorders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "OrdersInfos");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "OrdersInfos");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "OrdersInfos",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "OrdersInfos");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "OrdersInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "OrdersInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
