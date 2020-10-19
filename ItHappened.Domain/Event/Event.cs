using System;
using LanguageExt;

namespace ItHappened.Domain
{
    public class Event
    {
        public Guid Id { get; }
        public Guid CreatorId { get; }
        public Guid TrackerId { get; }
        public DateTimeOffset HappensDate { get; set; }
        public string Title { get; set; }
        public Option<Photo> Photo { get; set; }
        public Option<double> Scale { get; set; }
        public Option<double> Rating { get; set; }
        public Option<GeoTag> GeoTag { get; set; }
        public Option<Comment> Comment { get; set; }

        public Event(EventBuilder eventBuilder)
        {
            CreatorId = eventBuilder.CreatorId;
            Id = eventBuilder.Id;
            TrackerId = eventBuilder.TrackerId;
            Title = eventBuilder.Title;
            HappensDate = eventBuilder.HappensDate;
            Photo = eventBuilder.Photo;
            Scale = eventBuilder.Scale;
            Rating = eventBuilder.Rating;
            GeoTag = eventBuilder.GeoTag;
            Comment = eventBuilder.Comment;
        }
    }
}