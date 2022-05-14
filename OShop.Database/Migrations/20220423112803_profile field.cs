using Microsoft.EntityFrameworkCore.Migrations;

namespace OShop.Database.Migrations
{
    public partial class profilefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CompleteProfile",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteProfile",
                table: "AspNetUsers");
        }
    }
}
