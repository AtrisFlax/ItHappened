using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItHappened.Infrastructure.Migrations
{
    public partial class AddGeoTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeoTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GpsLat = table.Column<double>(nullable: false),
                    GpsLng = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoTags", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoTags");
        }
    }
}
