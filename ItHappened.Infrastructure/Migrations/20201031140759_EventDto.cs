using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItHappened.Infrastructure.Migrations
{
    public partial class EventDto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoTagDb");

            migrationBuilder.AddColumn<double>(
                name: "GpsLat",
                table: "EventRequests",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GpsLng",
                table: "EventRequests",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GpsLat",
                table: "EventRequests");

            migrationBuilder.DropColumn(
                name: "GpsLng",
                table: "EventRequests");

            migrationBuilder.CreateTable(
                name: "GeoTagDb",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventDbId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GpsLat = table.Column<double>(type: "float", nullable: false),
                    GpsLng = table.Column<double>(type: "float", nullable: false)
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
    }
}
