using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LivroManage.Database.Migrations
{
    public partial class newchanges2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserLocationId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserLocationId",
                table: "Orders");
        }
    }
}
