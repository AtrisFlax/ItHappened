using System;
using ItHappened.Domain;

namespace ItHappened.Api.Models.Responses
{
    public class EventGetResponse
    {
        public Guid Id { get; set; }
        public DateTimeOffset HappensDate { get; set; }
        public Photo Photo { get; set; }
        public double? Scale { get; set; }
        public double? Rating { get; set; }
        public GeoTag GeoTag { get; set; }
        public string Comment { get; set; }
    }
}