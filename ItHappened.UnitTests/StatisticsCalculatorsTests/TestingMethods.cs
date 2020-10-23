using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class TestingMethods
    {
        public static readonly Random Rand = new Random();

        public TestingMethods()
        {
        }

        public static EventTracker CreateTracker()
        {
            return new EventTracker(Guid.NewGuid(), Guid.NewGuid(), "Tracker name", new TrackerCustomizationSettings());
        }

        public static EventTracker CreateTrackerWithScale(string scale)
        {
            return new EventTracker(Guid.NewGuid(), Guid.NewGuid(), "Tracker name",
                new TrackerCustomizationSettings(
                    Option<string>.Some(scale),
                    false,
                    false,
                    false,
                    false
                ));
        }

        public static (IReadOnlyCollection<Event> events, List<double> Rating) CreateEventsWithRating(Guid trackerId,
            int num)
        {
            var ratings = CreateRandomRatings(num);
            var events = ratings.Select(t => CreateEventWithRating(trackerId, t)).ToList();
            return (events, ratings);
        }

        public static (IReadOnlyCollection<Event> events, List<double> Rating) CreateEventsWithCommentAndWithRating(
            Guid trackerId,
            int num)
        {
            var ratings = CreateRandomRatings(num);
            var events = ratings
                .Select((t, i) => CreateEventWithRatingAndComment(trackerId, t, new Comment($"Comment {i}")))
                .ToList()
                .AsReadOnly();
            return (events, ratings);
        }
        
        public static (IReadOnlyCollection<Event> events, List<double> Rating) CreateEventsWithCommentAndWithRatingInsideFromToTime(
            Guid trackerId,
            int createNumEvents,
            DateTimeOffset from, DateTimeOffset to)
        {
            var ratings = CreateRandomRatings(createNumEvents);
            var events = ratings
                .Select((t, i) => CreateEventWithRatingAndCommentInsideFromTo(trackerId, t, new Comment($"Comment {i}"), from, to))
                .ToList()
                .AsReadOnly();
            return (events, ratings);
        }

        public static (IReadOnlyCollection<Event> events, List<double> scale) CreateEventsWithScale(Guid trackerId,
            int num)
        {
            var scale = CreateRandomScale(num);
            var events = scale.Select(t => CreateEventWithScale(trackerId, t)).ToList();
            return (events, scale);
        }


        public static IEnumerable<Event> CreateEventsWithoutCustomization(Guid trackerId, int num)
        {
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(CreateEvent(trackerId));
            }

            return events;
        }

        public static List<double> CreateRandomRatings(int num)
        {
            var ratings = new List<double>();
            for (var i = 0; i < num; i++)
            {
                ratings.Add(Rand.NextDouble());
            }

            return ratings;
        }

        public static Event CreateEventWithRating(Guid trackerId, double rating)
        {
            return new Event(Guid.NewGuid(),
                Guid.NewGuid(),
                trackerId,
                DateTimeOffset.UtcNow,
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.None)
            );
        }

        public static Event CreateEventWithRatingAndComment(Guid trackerId, double rating, Comment comment)
        {
            return new Event(Guid.NewGuid(),
                Guid.NewGuid(),
                trackerId,
                DateTimeOffset.UtcNow,
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.Some(comment)
                )
            );
        }
        
        public static Event CreateEventWithRatingAndCommentInsideFromTo(Guid trackerId, double rating, Comment comment, DateTimeOffset from, DateTimeOffset to)
        {
            return new Event(Guid.NewGuid(),
                Guid.NewGuid(),
                trackerId,
                RandomDayFromTo(from, to),
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.Some(comment)
                )
            );
        }
        
        
        public static Event CreateEventWithRatingAndFixDate(Guid trackerId, double rating, Comment comment, DateTimeOffset fixDate)
        {
            return new Event(Guid.NewGuid(),
                Guid.NewGuid(),
                trackerId,
                fixDate,
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.None,
                    Option<double>.Some(rating),
                    Option<GeoTag>.None,
                    Option<Comment>.Some(comment)
                )
            );
        }
        private static DateTimeOffset RandomDayFromTo(DateTimeOffset from, DateTimeOffset to)
        {
            var range = (to - from).Days;
            return from.AddDays(Rand.Next(range));
        }
        
        public static Event CreateEventWithScale(Guid trackerId, double scale)
        {
            return new Event(Guid.NewGuid(),
                Guid.NewGuid(),
                trackerId,
                DateTimeOffset.UtcNow,
                new EventCustomParameters(
                    Option<Photo>.None,
                    Option<double>.Some(scale),
                    Option<double>.None,
                    Option<GeoTag>.None,
                    Option<Comment>.None)
            );
        }

        public static Event CreateEvent(Guid trackerId)
        {
            return new Event(Guid.NewGuid(),
                Guid.NewGuid(),
                trackerId,
                DateTimeOffset.UtcNow,
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
    }
}