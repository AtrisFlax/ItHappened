using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItHappened.Infrastructure.Migrations
{
    public partial class EventDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    HappensDate = table.Column<DateTimeOffset>(nullable: false),
                    Scale = table.Column<double>(nullable: true),
                    Rating = table.Column<double>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeoTagDb",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GpsLat = table.Column<double>(nullable: false),
                    GpsLng = table.Column<double>(nullable: false),
                    EventDbId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoTagDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeoTagDb_EventRequests_EventDbId",
                        column: x => x.EventDbId,
                        principalTable: "EventRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeoTagDb_EventDbId",
                table: "GeoTagDb",
                column: "EventDbId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoTagDb");

            migrationBuilder.DropTable(
                name: "EventRequests");
        }
    }
}
