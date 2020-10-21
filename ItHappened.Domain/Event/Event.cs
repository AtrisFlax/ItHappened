using System;
using LanguageExt;

namespace ItHappened.Domain
{
    public class Event
    {
        public Guid Id { get; }
        public Guid CreatorId { get; }
        public Guid TrackerId { get; }
        public DateTimeOffset HappensDate { get; private set; }
        public string Title { get; private set; }
        public Option<Photo> Photo { get; private set; }
        public Option<double> Scale { get; private set; }
        public Option<double> Rating { get; private set; }
        public Option<GeoTag> GeoTag { get; private set; }
        public Option<Comment> Comment { get; private set; }

        private Event(Guid id, Guid trackerId, Guid creatorId, DateTimeOffset happensDate, string title)
        {
            Id = id;
            TrackerId = trackerId;
            CreatorId = creatorId;
            HappensDate = happensDate;
            Title = title;
        }

        public static EventBuilder Create(Guid id, Guid trackerId, Guid creatorId, DateTimeOffset happensDate, string title)
            => new EventBuilder(new Event(id, trackerId, creatorId, happensDate, title));

        public class EventBuilder
        {
            private readonly Event _event;

            internal EventBuilder(Event @event)
            {
                _event = @event;
            }

            public EventBuilder WithPhoto(Photo photo)
            {
                _event.Photo = Option<Photo>.Some(photo);
                return this;
            }

            public EventBuilder WithScale(double scale)
            {
                _event.Scale = Option<double>.Some(scale);
                return this;
            }

            public EventBuilder WithRating(double rating)
            {
                _event.Rating = Option<double>.Some(rating);
                return this;
            }

            public EventBuilder WithGeoTag(GeoTag geoTag)
            {
                _event.GeoTag = Option<GeoTag>.Some(geoTag);
                return this;
            }

            public EventBuilder WithComment(string text)
            {
                _event.Comment = Option<Comment>.Some(new Comment(text));
                return this;
            }

            public Event Build() => _event;
        }
    }
}