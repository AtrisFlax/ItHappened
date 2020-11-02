using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItHappened.Infrastructure.Migrations
{
    public partial class FactsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BestEventCommentId",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BestEventDate",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BestRating",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventsCount",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationInDays",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "FirstEventAfterBreakDate",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastEventBeforeBreakDate",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DayWithLargestEventsCount",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MostEventfulDayTrackersFactDto_EventsCount",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MostEventfulWeekTrackersFactDto_EventsCount",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WeekWithLargestEventCountFirstDay",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WeekWithLargestEventCountLastDay",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EventsPeriod",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingName",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DaysOfTheWeek",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Percentage",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SingleTrackerEventsCountFactDto_EventsCount",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SpecificDayTimeFactDto_Percentage",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeOfTheDay",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SumValue",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorstEventCommentId",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WorstEventDate",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorstEventId",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WorstRating",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AverageScaleTrackerFactDto_MeasurementUnit",
                schema: "ItHappened",
                table: "Facts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facts_BestEventCommentId",
                schema: "ItHappened",
                table: "Facts",
                column: "BestEventCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Facts_WorstEventCommentId",
                schema: "ItHappened",
                table: "Facts",
                column: "WorstEventCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facts_Comment_BestEventCommentId",
                schema: "ItHappened",
                table: "Facts",
                column: "BestEventCommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Facts_Comment_WorstEventCommentId",
                schema: "ItHappened",
                table: "Facts",
                column: "WorstEventCommentId",
                principalTable: "Comment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facts_Comment_BestEventCommentId",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropForeignKey(
                name: "FK_Facts_Comment_WorstEventCommentId",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Facts_BestEventCommentId",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropIndex(
                name: "IX_Facts_WorstEventCommentId",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "BestEventCommentId",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "BestEventDate",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "BestRating",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "EventsCount",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "DurationInDays",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "FirstEventAfterBreakDate",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "LastEventBeforeBreakDate",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "DayWithLargestEventsCount",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "MostEventfulDayTrackersFactDto_EventsCount",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "MostEventfulWeekTrackersFactDto_EventsCount",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "WeekWithLargestEventCountFirstDay",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "WeekWithLargestEventCountLastDay",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "EventsPeriod",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "TrackingName",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "DaysOfTheWeek",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "Percentage",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "SingleTrackerEventsCountFactDto_EventsCount",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "SpecificDayTimeFactDto_Percentage",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "TimeOfTheDay",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "SumValue",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "WorstEventCommentId",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "WorstEventDate",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "WorstEventId",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "WorstRating",
                schema: "ItHappened",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "AverageScaleTrackerFactDto_MeasurementUnit",
                schema: "ItHappened",
                table: "Facts");
        }
    }
}
