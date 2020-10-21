//using System;
//using LanguageExt;

//namespace ItHappened.Domain
//{
//    public class EventBuilder
//    {
//        internal Option<Comment> Comment = Option<Comment>.None;
//        internal Option<GeoTag> GeoTag = Option<GeoTag>.None;
//        internal Option<Photo> Photo = Option<Photo>.None;
//        internal Option<double> Rating = Option<double>.None;
//        internal Option<double> Scale = Option<double>.None;
//        internal Guid Id { get; private set; }
//        internal Guid CreatorId { get; private set; }
//        internal Guid TrackerId { get; private set; }
//        internal DateTimeOffset HappensDate { get; private set; }
//        internal string Title { get; private set; }

//        public Event Build()
//        {
//            return new Event(this);
//        }

//        public static EventBuilder Event(Guid id, Guid creatorId, Guid trackerId, DateTimeOffset happensDate, string title)
//        {
//            return new EventBuilder
//            {
//                Id = id,
//                CreatorId = creatorId,
//                TrackerId = trackerId,
//                HappensDate = happensDate,
//                Title = title
//            };
//        }

//        public EventBuilder WithPhoto(Photo photo)
//        {
//            Photo = Option<Photo>.Some(photo);
//            return this;
//        }

//        public EventBuilder WithScale(double scale)
//        {
//            Scale = Option<double>.Some(scale);
//            return this;
//        }

//        public EventBuilder WithRating(double rating)
//        {
//            Rating = Option<double>.Some(rating);
//            return this;
//        }

//        public EventBuilder WithGeoTag(GeoTag geoTag)
//        {
//            GeoTag = Option<GeoTag>.Some(geoTag);
//            return this;
//        }

//        public EventBuilder WithComment(string text)
//        {
//            Comment = Option<Comment>.Some(new Comment(text));
//            return this;
//        }
//    }
//}