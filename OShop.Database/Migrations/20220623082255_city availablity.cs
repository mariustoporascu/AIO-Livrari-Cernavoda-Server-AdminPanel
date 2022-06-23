using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OShop.Database.Migrations
{
    public partial class cityavailablity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportFees_Companies_CompanieId",
                table: "TransportFees");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLocations_AspNetUsers_ApplicationUserId",
                table: "UserLocations");

            migrationBuilder.DropIndex(
                name: "IX_UserLocations_ApplicationUserId",
                table: "UserLocations");

            migrationBuilder.DropIndex(
                name: "IX_TransportFees_CompanieId",
                table: "TransportFees");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserLocations");

            migrationBuilder.DropColumn(
                name: "CompanieId",
                table: "TransportFees");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "AvailableCities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "AvailableCities");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserLocations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanieId",
                table: "TransportFees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_ApplicationUserId",
                table: "UserLocations",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportFees_CompanieId",
                table: "TransportFees",
                column: "CompanieId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportFees_Companies_CompanieId",
                table: "TransportFees",
                column: "CompanieId",
                principalTable: "Companies",
                principalColumn: "CompanieId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLocations_AspNetUsers_ApplicationUserId",
                table: "UserLocations",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
