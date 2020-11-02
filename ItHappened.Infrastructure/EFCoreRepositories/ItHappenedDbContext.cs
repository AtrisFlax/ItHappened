using ItHappened.Infrastructure.Dto;
using ItHappened.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
// ReSharper disable UnusedAutoPropertyAccessor.Global 

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class ItHappenedDbContext : DbContext
    {
        private const string Schema = "ItHappenedDB"; //TODO hardcoded  
        public DbSet<UserDto> Users { get; set; }
        public DbSet<EventTrackerDto> EventTrackers { get; set; }
        public DbSet<EventDto> Events { get; set; }


        //facts dto
        // ReSharper disable once UnusedMember.Global
        public DbSet<FactDto> FactsDto { get; set; } //abstract parent dto 
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
        public DbSet<SpecificDayTimeFactDto> SpecificDayTimeFactsDto { get; set; }
        public DbSet<SumScaleTrackerFactDto> SumScaleTrackerFactsDto { get; set; }
        public DbSet<WorstRatingEventFactDto> WorstRatingEventFactsDto { get; set; }

        public ItHappenedDbContext(DbContextOptions<ItHappenedDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventTrackerDto>(builder =>
            {
                builder.ToTable("EventTrackers", Schema);
            });
            
            modelBuilder.Entity<UserDto>(builder =>
            {
                builder.ToTable("Users", Schema);
                builder.HasMany<EventTrackerDto>().WithOne(tracker => tracker.UserDto).HasForeignKey("CreatorId");
            });
            
            modelBuilder.Entity<EventDto>(builder =>
            {
                builder.ToTable("Events", Schema);
            });


            modelBuilder.Entity<FactDto>().HasKey(fact => fact.Id);

            modelBuilder.Entity<FactDto>()
                .ToTable("Facts", Schema)
                .HasDiscriminator<int>("FactType")
                .HasValue<AverageRatingTrackerFactDto>(1)
                .HasValue<AverageScaleTrackerFactDto>(2)
                .HasValue<BestRatingEventFactDto>(3)
                .HasValue<EventsCountTrackersFactDto>(4)
                .HasValue<LongestBreakTrackerFactDto>(5)
                .HasValue<MostEventfulDayTrackersFactDto>(6)
                .HasValue<MostEventfulWeekTrackersFactDto>(7)
                .HasValue<MostFrequentEventTrackersFactDto>(8)
                .HasValue<OccursOnCertainDaysOfTheWeekTrackerFactDto>(9)
                .HasValue<SingleTrackerEventsCountFactDto>(10)
                .HasValue<SpecificDayTimeFactDto>(11)
                .HasValue<SumScaleTrackerFactDto>(12)
                .HasValue<WorstRatingEventFactDto>(13);
        }
    }
}