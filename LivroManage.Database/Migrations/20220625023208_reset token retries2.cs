using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace LivroManage.Database.Migrations
{
    public partial class resettokenretries2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiry",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetTokenExpiry",
                table: "AspNetUsers");
        }
    }
}
