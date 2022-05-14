using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class restaurantInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelefonNo",
                table: "Restaurante",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelefonNo",
                table: "Restaurante");
        }
    }
}
