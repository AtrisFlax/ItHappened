using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Dto;
using ItHappened.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class ItHappenedDbContext : DbContext
    {
        public DbSet<UserDto> Users { get; set; }
        public DbSet<EventTrackerDto> EventTrackers { get; set; }
        public DbSet<EventDto> Events { get; set; }

        
        //facts dto
        public DbSet<FactDto> FactsDto { get; set; }
        public DbSet<AverageRatingTrackerFactDto> AverageRatingTrackerFactsDto { get; set; }
        public DbSet<AverageScaleTrackerFactDto> AverageScaleTrackerFactsDto { get; set; }
        public DbSet<BestRatingEventFactDto> BestRatingEventFactsDto { get; set; }
        public DbSet<EventsCountTrackersFactDto> EventsCountTrackersFactsDto { get; set; }
        public DbSet<LongestBreakTrackerFactDto> LongestBreakTrackerFactsDto { get; set; }
        public DbSet<MostEventfulDayTrackersFactDto> MostEventfulDayTrackersFactsDto { get; set; }
        public DbSet<MostEventfulWeekTrackersFactDto> MostEventfulWeekTrackersFactsDto { get; set; }
        public DbSet<MostFrequentEventTrackersFactDto> MostFrequentEventTrackersFactsDto { get; set; }
        public DbSet<OccursOnCertainDaysOfTheWeekTrackerFactDto> OccursOnCertainDaysOfTheWeekTrackerFactsDto { get; set; }
        public DbSet<SingleTrackerEventsCountFactDto> SingleTrackerEventsCountFactsDto { get; set; }
        public DbSet<SpecificDayTimeEventFactDto> SpecificDayTimeEventFactsDto { get; set; }
        public DbSet<SumScaleTrackerFactDto> SumScaleTrackerFactsDto { get; set; }
        public DbSet<WorstRatingEventFactDto> WorstRatingEventFactsDto { get; set; }
        
        public ItHappenedDbContext(DbContextOptions<ItHappenedDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region map order

            //TODO test 
            modelBuilder.Entity<EventTrackerDto>(builder =>
            {
                builder.ToTable("EventTrackers", "ItHappenedDB");
                //builder.HasMany<EventDto>().WithOne(@event => @event.EventTrackerDto).HasForeignKey("TrackerId");
            });

            modelBuilder.Entity<UserDto>(builder =>
            {
                builder.ToTable("Users", "ItHappenedDB");
                builder.HasMany<EventTrackerDto>().WithOne(tracker => tracker.UserDto).HasForeignKey("CreatorId");
            });

            modelBuilder.Entity<EventDto>(builder =>
            {
                builder.ToTable("Events", "ItHappenedDB");
                //builder.HasMany<EventDto>().WithOne(@event => @event.EventTrackerDto).HasForeignKey("TrackerId");
            });

            
            //
            // modelBuilder.Entity<Blog>()
            //     .HasDiscriminator<string>("blog_type")
            //     .HasValue<Blog>("blog_base")
            //     .HasValue<RssBlog>("blog_rss");

            modelBuilder.Entity<FactDto>()
                .ToTable("Facts")
                .HasDiscriminator<string>("FactType")
                .HasValue<AverageRatingTrackerFactDto>("AverageRatingTrackerFact")
                .HasValue<AverageScaleTrackerFactDto>("AverageScaleTrackerFact")
                .HasValue<BestRatingEventFactDto>("BestRatingEventFact")
                .HasValue<EventsCountTrackersFactDto>("EventsCountTrackersFact")
                .HasValue<LongestBreakTrackerFactDto>("LongestBreakTrackerFact")
                .HasValue<MostEventfulDayTrackersFactDto>("MostEventfulDayTrackersFact")
                .HasValue<MostEventfulWeekTrackersFactDto>("MostEventfulWeekTrackersFact")
                .HasValue<MostFrequentEventTrackersFactDto>("MostFrequentEventTrackersFact")
                .HasValue<OccursOnCertainDaysOfTheWeekTrackerFactDto>("OccursOnCertainDaysOfTheWeekTrackerFact")
                .HasValue<SingleTrackerEventsCountFactDto>("SingleTrackerEventsCountFact")
                .HasValue<SpecificDayTimeEventFactDto>("SpecificDayTimeEventFact")
                .HasValue<SumScaleTrackerFactDto>("SumScaleTrackerFact");

            #endregion
        }
    }
}