using System;

namespace ItHappend.Domain
{
    public class EventBuilder
    {
        public Guid Id { get; private set; }
        public Guid CreatorId { get; private set; }
        public DateTimeOffset HappensDate { get; private set; }
        public string Title { get; private set; }

        public double Evaluation { get; private set; }


        internal Optional<Photo> Photo = Optional<Photo>.None();
        internal Optional<Scale> Scale = Optional<Scale>.None();
        internal Optional<Rating> Rating = Optional<Rating>.None();
        internal Optional<GeoTag> GeoTag = Optional<GeoTag>.None();
        internal Optional<Comment> Comment = Optional<Comment>.None();

        public Event Build()
        {
            return new Event(this);
        }

        public static EventBuilder Event(Guid id, Guid creatorId, DateTimeOffset happensDate, string title,
            double evaluation)
        {
            if (title == null) throw new NullReferenceException();
            return new EventBuilder
            {
                Id = id,
                CreatorId = creatorId,
                HappensDate = happensDate,
                Title = title,
                Evaluation = evaluation
            };
        }

        public EventBuilder WithPhoto(byte[] photoBytes)
        {
            Photo = Optional<Photo>.Some(new Photo(photoBytes));
            return this;
        }

        public EventBuilder WithScale(double scale)
        {
            Scale = Optional<Scale>.Some(new Scale(scale));
            return this;
        }

        public EventBuilder WithRating(double rating)
        {
            Rating = Optional<Rating>.Some(new Rating(rating));
            return this;
        }

        public EventBuilder WithGeoTag(GeoTag geoTag)
        {
            GeoTag = Optional<GeoTag>.Some(geoTag);
            return this;
        }

        public EventBuilder WithComment(string text)
        {
            Comment = Optional<Comment>.Some(new Comment(text));
            return this;
        }
    }
}