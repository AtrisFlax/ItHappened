using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItHappened.Infrastructure.Mappers
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public Guid TrackerId { get; set; }
        public DateTimeOffset HappensDate { get; set; }
      
        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }
        public double? Scale { get; set; }
        public double? Rating { get; set; }
        public double? LatitudeGeo { get; set; }
        public double? LongitudeGeo { get; set; }
        public string Comment { get; set; }
    }
}