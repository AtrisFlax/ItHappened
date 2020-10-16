using System;
using ItHappend.Domain.EventCustomization;
using LanguageExt;

namespace ItHappend.Domain
{
    public class EventBuilder
    {
        public Guid Id { get; private set; }
        public Guid CreatorId { get; private set; }
        public DateTimeOffset HappensDate { get; private set; }
        public string Title { get; private set; }

        internal Option<Photo> Photo = Option<Photo>.None;
        internal Option<double> Scale = Option<double>.None;
        internal Option<double> Rating = Option<double>.None;
        internal Option<GeoTag> GeoTag = Option<GeoTag>.None;
        internal Option<Comment> Comment = Option<Comment>.None;

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
            Photo = Option<Photo>.Some(photo);
            return this;
        }
        
        public EventBuilder WithScale(double scale)
        {
            Scale = Option<double>.Some(scale);
            return this;
        }

        public EventBuilder WithRating(double rating)
        {
            Rating = Option<double>.Some(rating);
            return this;
        }

        public EventBuilder WithGeoTag(GeoTag geoTag)
        {
            GeoTag = Option<GeoTag>.Some(geoTag);
            return this;
        }

        public EventBuilder WithComment(string text)
        {
            Comment = Option<Comment>.Some(new Comment(text));
            return this;
        }
    }
}