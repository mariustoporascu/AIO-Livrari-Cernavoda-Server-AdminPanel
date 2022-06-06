using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OShop.Database.Migrations
{
    public partial class linkusertofbtokenandlocationsfordelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserLocations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FBTokens",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_UserId",
                table: "UserLocations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FBTokens_UserId",
                table: "FBTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FBTokens_AspNetUsers_UserId",
                table: "FBTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocations_AspNetUsers_UserId",
                table: "UserLocations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FBTokens_AspNetUsers_UserId",
                table: "FBTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLocations_AspNetUsers_UserId",
                table: "UserLocations");

            migrationBuilder.DropIndex(
                name: "IX_UserLocations_UserId",
                table: "UserLocations");

            migrationBuilder.DropIndex(
                name: "IX_FBTokens_UserId",
                table: "FBTokens");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserLocations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FBTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
