using Microsoft.EntityFrameworkCore.Migrations;

namespace CarSalesSystem.Migrations
{
    public partial class AddedAdvertisementIdColumnInVehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisements_VehicleId",
                table: "Advertisements");

            migrationBuilder.AddColumn<string>(
                name: "AdvertisementId",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_VehicleId",
                table: "Advertisements",
                column: "VehicleId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisements_VehicleId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "AdvertisementId",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_VehicleId",
                table: "Advertisements",
                column: "VehicleId");
        }
    }
}
