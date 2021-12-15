using Microsoft.EntityFrameworkCore.Migrations;

namespace CarSalesSystem.Migrations
{
    public partial class FixedRelationBetweenAdvertisementAndDealership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_CarDealerShips_CarDealerShipId",
                table: "Advertisements");

            migrationBuilder.RenameColumn(
                name: "CarDealerShipId",
                table: "Advertisements",
                newName: "CarDealershipId");

            migrationBuilder.RenameIndex(
                name: "IX_Advertisements_CarDealerShipId",
                table: "Advertisements",
                newName: "IX_Advertisements_CarDealershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_CarDealerShips_CarDealershipId",
                table: "Advertisements",
                column: "CarDealershipId",
                principalTable: "CarDealerShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_CarDealerShips_CarDealershipId",
                table: "Advertisements");

            migrationBuilder.RenameColumn(
                name: "CarDealershipId",
                table: "Advertisements",
                newName: "CarDealerShipId");

            migrationBuilder.RenameIndex(
                name: "IX_Advertisements_CarDealershipId",
                table: "Advertisements",
                newName: "IX_Advertisements_CarDealerShipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_CarDealerShips_CarDealerShipId",
                table: "Advertisements",
                column: "CarDealerShipId",
                principalTable: "CarDealerShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
