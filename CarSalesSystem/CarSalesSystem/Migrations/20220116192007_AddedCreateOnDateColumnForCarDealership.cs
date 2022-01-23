using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarSalesSystem.Migrations
{
    public partial class AddedCreateOnDateColumnForCarDealership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisements_VehicleId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "AdvertisementId",
                table: "Vehicles");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CarDealerShips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_VehicleId",
                table: "Advertisements",
                column: "VehicleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Advertisements_VehicleId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CarDealerShips");

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
    }
}
