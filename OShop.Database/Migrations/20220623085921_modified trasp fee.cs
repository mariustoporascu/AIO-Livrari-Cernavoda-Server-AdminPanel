using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OShop.Database.Migrations
{
    public partial class modifiedtraspfee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransportFees_AvailableCities_CompanieRefId",
                table: "TransportFees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransportFees",
                table: "TransportFees");

            migrationBuilder.DropIndex(
                name: "IX_TransportFees_CompanieRefId",
                table: "TransportFees");

            migrationBuilder.RenameColumn(
                name: "CompanieRefId",
                table: "TransportFees",
                newName: "MainCityRefId");

            migrationBuilder.AddColumn<int>(
                name: "TranspFeeId",
                table: "TransportFees",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 1)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransportFees",
                table: "TransportFees",
                column: "TranspFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportFees_CityRefId",
                table: "TransportFees",
                column: "CityRefId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransportFees",
                table: "TransportFees");

            migrationBuilder.DropIndex(
                name: "IX_TransportFees_CityRefId",
                table: "TransportFees");

            migrationBuilder.DropColumn(
                name: "TranspFeeId",
                table: "TransportFees");

            migrationBuilder.RenameColumn(
                name: "MainCityRefId",
                table: "TransportFees",
                newName: "CompanieRefId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransportFees",
                table: "TransportFees",
                columns: new[] { "CityRefId", "CompanieRefId" });

            migrationBuilder.CreateIndex(
                name: "IX_TransportFees_CompanieRefId",
                table: "TransportFees",
                column: "CompanieRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransportFees_AvailableCities_CompanieRefId",
                table: "TransportFees",
                column: "CompanieRefId",
                principalTable: "AvailableCities",
                principalColumn: "CityId");
        }
    }
}
