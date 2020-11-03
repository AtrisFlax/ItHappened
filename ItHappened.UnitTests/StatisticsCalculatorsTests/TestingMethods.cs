using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConstants;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public static class TestingMethods
    {
        public static readonly Random Rand = new Random();


        public static EventTracker CreateTrackerWithDefaultCustomization(Guid userId, string name = "Tracker name")
        {
            return new EventTracker(Guid.NewGuid(), userId, name, new TrackerCustomizationSettings());
        }

        public static EventTracker CreateTrackerWithRequiredCustomization(Guid userId, string name,
            TrackerCustomizationSettings trackerCustomizationSettings)
        {
            return new EventTracker(Guid.NewGuid(), userId, name,
                trackerCustomizationSettings);
        }

        public static EventTracker CreateTrackerWithScale(Guid userId, string scale)
        {
            return new EventTracker(userId, Guid.NewGuid(), "Tracker name",
                new TrackerCustomizationSettings(false,
                    true,
                    scale,
                    false,
                    false,
                    false, false));
        }

        public static (IReadOnlyCollection<Event> events, List<double> Rating) CreateEventsWithRating(Guid trackerId,
            Guid userId,
            int num)
        {
            var ratings = CreateRandomRatings(num);
            var events = ratings.Select(t => CreateEventWithRating(trackerId, userId, t)).ToList();
            return (events, ratings);
        }

        public static (IReadOnlyCollection<Event> events, IReadOnlyCollection<double> Rating,
            IReadOnlyCollection<Comment> comments)
            CreateEventsWithCommentAndWithRatingFromTo(Guid trackerId,
                Guid creatorId,
                int num, DateTimeOffset from, DateTimeOffset to)
        {
            var ratings = CreateRandomRatings(num);
            var comments = CreateRandomComments(num);
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(CreateEventWithRatingAndCommentInsideFromTo(trackerId, creatorId, ratings[i], comments[i],
                    from, to));
            }

            return (events.AsReadOnly(), ratings.AsReadOnly(), comments.AsReadOnly());
        }

        public static (IReadOnlyCollection<Event> events, IReadOnlyCollection<double> Rating,
            IReadOnlyCollection<Comment> comments)
            CreateEventsWithCommentAndWithRating(Guid trackerId,
                Guid creatorId,
                int num)
        {
            var ratings = CreateRandomRatings(num);
            var comments = CreateRandomComments(num);
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(CreateEventWithRatingAndComment(trackerId, creatorId, ratings[i], comments[i]));
            }

            return (events.AsReadOnly(), ratings.AsReadOnly(), comments.AsReadOnly());
        }

        private static List<Comment> CreateRandomComments(int num)
        {
            var comments = new List<Comment>();
            for (var i = 0; i < num; i++)
            {
                comments.Add(new Comment($"Comment {i}"));
            }

            return comments;
        }

        public static (IReadOnlyCollection<Event> events, List<double> Rating)
            CreateEventsWithCommentAndWithRatingInsideFromToTime(
                Guid trackerId,
                Guid creatorId,
                int createNumEvents,
                DateTimeOffset from, DateTimeOffset to)
        {
            var ratings = CreateRandomRatings(createNumEvents);
            var events = ratings
                .Select((rating, i) =>
                    CreateEventWithRatingAndCommentInsideFromTo(trackerId, creatorId, rating,
                        new Comment($"Comment {i}"), from, to))
                .ToList()
                .AsReadOnly();
            return (events, ratings);
        }

        public static (IReadOnlyCollection<Event> events, List<double> scale) CreateEventsWithScale(Guid trackerId,
            int num)
        {
            var scale = CreateRandomScale(num);
            var events = scale.Select(t => CreateEventWithScale(trackerId, Guid.NewGuid(), t)).ToList();
            return (events, scale);
        }


        public static IEnumerable<Event> CreateEventsWithoutCustomization(Guid trackerId, Guid userId, int num)
        {
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(CreateEvent(trackerId, userId));
            }

            return events;
        }

        public static List<double> CreateRandomRatings(int num)
        {
            var ratings = new List<double>();
            for (var i = 0; i < num; i++)
            {
                ratings.Add(Rand.NextDouble() % MaxRatingValue);
            }

            return ratings;
        }

        public static Event CreateEventWithRating(Guid trackerId, Guid userId, double rating)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                DateTimeOffset.UtcNow, "Event title", 
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.None)
            );
        }

        public static Event CreateEventWithRating(Guid trackerId, Guid userId, DateTimeOffset dateTime, double rating)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                dateTime, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.None)
            );
        }

        public static Event CreateEventWithComment(Guid trackerId, Guid userId, string comment)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                DateTimeOffset.UtcNow, "Event title", 
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.None,
                    Option<GeoTag>.None,
                    Option<Comment>.Some(new Comment(comment)))
            );
        }


        public static Event CreateEventWithComment(Guid trackerId, Guid userId, DateTimeOffset dateTime, string comment)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                dateTime, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.None,
                    Option<GeoTag>.None,
                    Option<Comment>.Some(new Comment(comment)))
            );
        }

        public static Event CreateEventWithGeoTag(Guid trackerId, Guid userId, GeoTag geoTag)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                DateTimeOffset.UtcNow, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.None,
                    Option<GeoTag>.Some(geoTag),
                    Option<Comment>.None)
            );
        }

        public static Event CreateEventWithGeoTag(Guid trackerId, Guid userId, DateTimeOffset time, GeoTag geoTag)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                time, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.None,
                    Option<GeoTag>.Some(geoTag),
                    Option<Comment>.None)
            );
        }

        public static Event CreateEventWithAll(Guid trackerId, Guid userId, DateTimeOffset time,
            Photo photo, double scale, double rating, GeoTag geoTag, Comment comment)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                time, "Event title",
                new EventCustomParameters(
                    Option<Photo>.Some(photo),
                    Option<double>.Some(scale),
                    Option<double>.Some(rating),
                    Option<GeoTag>.Some(geoTag),
                    Option<Comment>.Some(comment))
            );
        }


        public static Event CreateEventWithRatingAndComment(Guid trackerId, Guid creatorId, double rating,
            Comment comment)
        {
            return new Event(Guid.NewGuid(),
                creatorId,
                trackerId,
                DateTimeOffset.UtcNow, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.Some(comment)
                )
            );
        }


        public static Event CreateEventWithRatingAndCommentFromTo(Guid trackerId, Guid creatorId, double rating,
            Comment comment, DateTimeOffset from, DateTimeOffset to)
        {
            return new Event(Guid.NewGuid(),
                creatorId,
                trackerId,
                RandomDayFromTo(from, to), "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.Some(comment)
                )
            );
        }


        public static Event CreateEventWithRatingAndCommentInsideFromTo(Guid trackerId, Guid creatorId, double rating,
            Comment comment,
            DateTimeOffset from, DateTimeOffset to)
        {
            return new Event(Guid.NewGuid(),
                creatorId,
                trackerId,
                RandomDayFromTo(from, to), "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.Some(comment)
                )
            );
        }


        public static Event CreateEventWithRatingWithCommentAndFixDate(Guid trackerId, Guid userId, double rating,
            Comment comment,
            DateTimeOffset fixDate)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                fixDate, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.Some(comment)
                )
            );
        }

        public static Event CreateEventFixDate(Guid trackerId, Guid userId, DateTimeOffset fixDate)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                fixDate, "Event title",
                new EventCustomParameters()
            );
        }

        public static Event CreateEventWithRatingAndFixDate(Guid trackerId, Guid userId, double rating,
            DateTimeOffset fixDate)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                fixDate, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.None
                )
            );
        }


        public static Event CreateEventWithFixTime(Guid trackerId, Guid userId, DateTimeOffset fixTime)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                fixTime, "Event title",
                new EventCustomParameters()
            );
        }

        private static DateTimeOffset RandomDayFromTo(DateTimeOffset from, DateTimeOffset to)
        {
            var range = (to - from).Days;
            var randomDay = from.AddDays(Rand.Next(range));
            return randomDay;
        }

        public static Event CreateEventWithScale(Guid trackerId, Guid creatorId, double scale)
        {
            return new Event(Guid.NewGuid(),
                creatorId,
                trackerId,
                DateTimeOffset.UtcNow, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.Some(scale),
                    Option<double>.None,
                    Option<GeoTag>.None,
                    Option<Comment>.None)
            );
        }

        public static Event CreateEventWithScale(Guid trackerId, Guid creatorId, DateTimeOffset dateTime, double scale)
        {
            return new Event(Guid.NewGuid(),
                creatorId,
                trackerId,
                dateTime, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.Some(scale),
                    Option<double>.None,
                    Option<GeoTag>.None,
                    Option<Comment>.None)
            );
        }

        public static Event CreateEvent(Guid trackerId, Guid userId)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                DateTimeOffset.UtcNow, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.None,
                    Option<GeoTag>.None,
                    Option<Comment>.None)
            );
        }

        public static Event CreateEvent(Guid trackerId, Guid userId, DateTimeOffset dateTime)
        {
            return new Event(Guid.NewGuid(),
                userId,
                trackerId,
                dateTime, "Event title",
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.None,
                    Option<GeoTag>.None,
                    Option<Comment>.None)
            );
        }


        private static List<double> CreateRandomScale(int num)
        {
            var scaleValues = new List<double>();
            for (var i = 0; i < num; i++)
            {
                scaleValues.Add(Rand.NextDouble());
            }

            return scaleValues;
        }

        public static IEnumerable<Event> CreateEventsEveryDayByDayInPast(Guid trackerId, Guid userId, int num,
            DateTimeOffset time)
        {
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(CreateEventWithFixTime(trackerId, userId, time.AddDays(-i)));
            }

            return events;
        }

        public static IEnumerable<Event> CreateEventsEveryDaysAgo(Guid eventTrackerId, Guid userId, int daysAgo,
            int num, DateTimeOffset now)
        {
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(CreateEventWithFixTime(eventTrackerId, userId, now.AddDays(-daysAgo)));
            }

            return events;
        }

        public static IReadOnlyCollection<Event> CreateEvents(Guid eventTrackerId, Guid userId, int num)
        {
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(CreateEvent(eventTrackerId, userId));
            }

            return events;
        }
    }
}