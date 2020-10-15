using System;
using Optional;

namespace ItHappend.Domain
{
    public class EventBuilder : IEventBuilder
    {
        public Guid Id { get; private set; }
        public Guid CreatorId { get; private set; }
        public DateTimeOffset HappensDate { get; private set; }
        public string Title { get; private set; }
        
        internal Option<Photo> Photo = Option.None<Photo>();
        internal Option<double> Scale = Option.None<double>();
        internal Option<double> Rating = Option.None<double>();
        internal Option<GeoTag> GeoTag = Option.None<GeoTag>();
        internal Option<string> Comment = Option.None<string>();

        public Event Build()
        {
            return new Event(this);
        }

        public static EventBuilder Event(Guid id, Guid creatorId, DateTimeOffset happensDate, string title)
        {
            if (title == null) throw new NullReferenceException();
            return new EventBuilder
            {
                Id = id,
                CreatorId = creatorId,
                HappensDate = happensDate,
                Title = title,
            };
        }

        public EventBuilder WithPhoto(Photo photo)
        {
            Photo = Option.Some(photo);
            return this;
        }

        public EventBuilder WithScale(double scale)
        {
            Scale = Option.Some(scale);
            return this;
        }

        public EventBuilder WithRating(double rating)
        {
            Rating = Option.Some(rating);
            return this;
        }
        
        public EventBuilder WithGeoTag(GeoTag geoTag)
        {
            GeoTag = Option.Some(geoTag);
            return this;
        }

        public EventBuilder WithComment(string comment)
        {
            Comment = Option.Some(comment);
            return this;
        }
    }
}