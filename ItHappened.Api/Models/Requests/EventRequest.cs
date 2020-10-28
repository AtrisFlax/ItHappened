using System;

namespace ItHappened.Api.Models.Requests
{
    public class EventRequest
    {
        public DateTimeOffset HappensDate { get; set; }
        //public PhotoRequest Photo { get; set; }
        public double? Scale { get; set; }
        public double? Rating { get; set; }
        public GeoTagRequest GeoTag { get; set; }
        public string Comment { get; set; }
    }
}