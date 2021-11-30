using Microsoft.EntityFrameworkCore.Migrations;

namespace CarSalesSystem.Migrations
{
    public partial class RelationBetweenUsersAndAdvertisementsAndCarDealerships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CarDealerShips",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Advertisements",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CarDealerShips_UserId",
                table: "CarDealerShips",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_UserId",
                table: "Advertisements",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_AspNetUsers_UserId",
                table: "Advertisements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarDealerShips_AspNetUsers_UserId",
                table: "CarDealerShips",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_AspNetUsers_UserId",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_CarDealerShips_AspNetUsers_UserId",
                table: "CarDealerShips");

            migrationBuilder.DropIndex(
                name: "IX_CarDealerShips_UserId",
                table: "CarDealerShips");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_UserId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CarDealerShips");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Advertisements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
