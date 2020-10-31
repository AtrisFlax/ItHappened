using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItHappened.Infrastructure.Migrations
{
    public partial class RenameEventsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventRequests");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    HappensDate = table.Column<DateTimeOffset>(nullable: false),
                    Scale = table.Column<double>(nullable: true),
                    Rating = table.Column<double>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    GpsLat = table.Column<double>(nullable: false),
                    GpsLng = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.CreateTable(
                name: "EventRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GpsLat = table.Column<double>(type: "float", nullable: false),
                    GpsLng = table.Column<double>(type: "float", nullable: false),
                    HappensDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: true),
                    Scale = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRequests", x => x.Id);
                });
        }
    }
}
