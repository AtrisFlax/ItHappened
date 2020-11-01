using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class ItHappenedDbContext : DbContext
    {
        public DbSet<UserDto> Users { get; set; }
        public DbSet<EventTrackerDto> EventTrackers { get; set; }
        public DbSet<EventDto> Events { get; set; }

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
            #endregion
            
        }
    }
}