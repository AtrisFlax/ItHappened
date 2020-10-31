// using System;
// using Microsoft.EntityFrameworkCore.Migrations;
//
// namespace ItHappened.Infrastructure.Migrations
// {
//     public partial class Events : Migration
//     {
//         protected override void Up(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.CreateTable(
//                 name: "Events",
//                 columns: table => new
//                 {
//                     Id = table.Column<Guid>(nullable: false),
//                     CreatorId = table.Column<Guid>(nullable: false),
//                     TrackerId = table.Column<Guid>(nullable: false),
//                     HappensDate = table.Column<DateTimeOffset>(nullable: false)
//                 },
//                 constraints: table =>
//                 {
//                     table.PrimaryKey("PK_Events", x => x.Id);
//                 });
//         }
//
//         protected override void Down(MigrationBuilder migrationBuilder)
//         {
//             migrationBuilder.DropTable(
//                 name: "Events");
//         }
//     }
// }
