using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OShop.Database.Migrations
{
    public partial class newchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumOrderValue",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "TransporFee",
                table: "Companies");

            migrationBuilder.AddColumn<bool>(
                name: "TelephoneOrdered",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TransportFees",
                columns: table => new
                {
                    CompanieRefId = table.Column<int>(type: "int", nullable: false),
                    CityRefId = table.Column<int>(type: "int", nullable: false),
                    TransporFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumOrderValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportFees", x => new { x.CityRefId, x.CompanieRefId });
                    table.ForeignKey(
                        name: "FK_TransportFees_AvailableCities_CityRefId",
                        column: x => x.CityRefId,
                        principalTable: "AvailableCities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransportFees_Companies_CompanieRefId",
                        column: x => x.CompanieRefId,
                        principalTable: "Companies",
                        principalColumn: "CompanieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportFees_CompanieRefId",
                table: "TransportFees",
                column: "CompanieRefId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportFees");

            migrationBuilder.DropColumn(
                name: "TelephoneOrdered",
                table: "Orders");

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumOrderValue",
                table: "Companies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransporFee",
                table: "Companies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
