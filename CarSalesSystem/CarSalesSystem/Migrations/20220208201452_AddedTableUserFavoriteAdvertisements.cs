using Microsoft.EntityFrameworkCore.Migrations;

namespace CarSalesSystem.Migrations
{
    public partial class AddedTableUserFavoriteAdvertisements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFavAdvertisements",
                columns: table => new
                {
                    AdvertisementId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdvertisementId1 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavAdvertisements", x => new { x.AdvertisementId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserFavAdvertisements_AspNetUsers_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavAdvertisements_AdvertisementId1",
                table: "UserFavAdvertisements",
                column: "AdvertisementId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
