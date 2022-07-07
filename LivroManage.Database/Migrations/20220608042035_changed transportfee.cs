using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LivroManage.Database.Migrations
{
    public partial class changedtransportfee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportFees_Companies_CompanieRefId",
                table: "TransportFees");

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

            migrationBuilder.AddColumn<int>(
                name: "TipCompanieRefId",
                table: "TransportFees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FBTokens",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_ApplicationUserId",
                table: "UserLocations",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportFees_CompanieId",
                table: "TransportFees",
                column: "CompanieId");

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
                name: "FK_TransportFees_AvailableCities_CompanieRefId",
                table: "TransportFees",
                column: "CompanieRefId",
                principalTable: "AvailableCities",
                principalColumn: "CityId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FBTokens_AspNetUsers_UserId",
                table: "FBTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_TransportFees_AvailableCities_CompanieRefId",
                table: "TransportFees");

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

            migrationBuilder.DropIndex(
                name: "IX_FBTokens_UserId",
                table: "FBTokens");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserLocations");

            migrationBuilder.DropColumn(
                name: "CompanieId",
                table: "TransportFees");

            migrationBuilder.DropColumn(
                name: "TipCompanieRefId",
                table: "TransportFees");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FBTokens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TransportFees_Companies_CompanieRefId",
                table: "TransportFees",
                column: "CompanieRefId",
                principalTable: "Companies",
                principalColumn: "CompanieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
