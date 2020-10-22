using System;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Api.Contracts.Responses
{
    public class EventResponse
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public Guid TrackerId { get; set; }
        public DateTimeOffset HappensDate { get; set; }
        public string Title { get; set; }
        public Option<Photo> Photo { get; set; }
        public Option<double> Scale { get; set; }
        public Option<double> Rating { get; set; }
        public Option<GeoTag> GeoTag { get; set; }
        public Option<Comment> Comment { get; set; }
    }
}