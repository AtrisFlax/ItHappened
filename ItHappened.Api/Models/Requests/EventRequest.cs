using System;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Api.Models.Requests
{
    public class EventRequest
    {
        public DateTimeOffset HappensDate { get; set; }
        public Option<Photo> Photo { get; set; }
        public Option<double> Scale { get; set; }
        public Option<double> Rating { get; set; }
        public Option<GeoTag> GeoTag { get; set; }
        public Option<Comment> Comment { get; set; }
    }
}