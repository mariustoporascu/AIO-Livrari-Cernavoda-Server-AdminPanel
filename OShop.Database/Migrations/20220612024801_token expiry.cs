using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OShop.Database.Migrations
{
    public partial class tokenexpiry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LoginTokenExpiry",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginTokenExpiry",
                table: "AspNetUsers");
        }
    }
}
