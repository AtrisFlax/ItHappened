using System;
using ItHappened.Domain;

using Microsoft.EntityFrameworkCore;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class ItHappenedDbContext : DbContext
    {
        
        public ItHappenedDbContext(DbContextOptions<ItHappenedDbContext> options) : base(options) { }
        
        public DbSet<EventDto> Events { get; set; }


    }

    public class EventDto
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public Guid TrackerId { get; set; }
        public DateTimeOffset HappensDate { get; set; }
        public double? Scale { get; set; }
        public double? Rating { get; set; }
        public string Comment { get; set; }
        public double GpsLat { get; set; }
        public double GpsLng { get; set; }
    }
}