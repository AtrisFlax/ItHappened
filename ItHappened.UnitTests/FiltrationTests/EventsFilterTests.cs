using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;

namespace ItHappened.UnitTests.FiltrationTests
{
    public class EventsFilterTests
    {
        private IEventsFilter _commentFilter;
        private IEventsFilter _geoTagFilter;
        private IEventsFilter _ratingFilter;
        private IEventsFilter _dateTimeFilter;
        private IEventsFilter _scaleFilter;
        private IReadOnlyCollection<IEventsFilter> _filters;
        private DateTimeOffset _from;
        private DateTimeOffset _to;

        [SetUp]
        public void Init()
        {
            _from = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero);
            _to = new DateTimeOffset(2020, 10, 14, 18, 01, 00, TimeSpan.Zero);
            _commentFilter = new CommentFilter("substringFilter", "Вст");
            _geoTagFilter = new GeoTagFilter("geotag", new GeoTag(10, 15), new GeoTag(20, 25));
            _ratingFilter = new RatingFilter("rating", 5.5, 8.6);
            _dateTimeFilter = new DateTimeFilter("DateTime", _from, _to);
            _scaleFilter = new ScaleFilter("scale", 5.5, 8.6);
            _filters = new[] {_commentFilter, _ratingFilter, _scaleFilter, _dateTimeFilter, _geoTagFilter};
        }

        [Test]
        public void FilterAllEventsNotPass_ReturnEmptyCollection()
        {
            //arrange
            var time = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero);
            var trackerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var events = new List<Event>
            {
                CreateEventWithComment(trackerId, userId, time, "привет!"),
                CreateEventWithScale(trackerId, userId, time, 1.0),
                CreateEventWithFixTime(trackerId, userId, DateTimeOffset.Now),
                CreateEventWithRating(trackerId, userId, time, 1.0),
                CreateEventWithGeoTag(trackerId, userId, time, new GeoTag(0, 0)),
            };

            //act
            var filteredEvents = EventsFilter.Filter(events, _filters).ToList();

            //assert
            Assert.AreEqual(0, filteredEvents.Count);
        }

        [Test]
        public void FilterAllEventsPass_ReturnAllCollection()
        {
            //arrange
            var happensDate = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero);
            var trackerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var events = new List<Event>
            {
                //filter out by scale
                CreateEventWithAll(trackerId, userId, happensDate, new Photo(new byte[] {0x1}), 1, 5.6, new GeoTag(11.0, 16.0),
                    new Comment("здравствуй!")),
                //filter out by happensDate
                CreateEventWithAll(trackerId, userId, DateTimeOffset.Now, new Photo(new byte[] {0x1}), 8, 5.6, new GeoTag(11.0, 16.0),
                    new Comment("здравствуй!")),
                //filter out by comment content
                CreateEventWithAll(trackerId, userId, happensDate, new Photo(new byte[] {0x1}), 8, 5.6, new GeoTag(11.0, 16.0),
                    new Comment("Привет!")),
                //filter out by GeoTag
                CreateEventWithAll(trackerId, userId, happensDate, new Photo(new byte[] {0x1}), 8, 5.6, new GeoTag(0, 0),
                    new Comment("здравствуй!")),
                //filter out by rating
                CreateEventWithAll(trackerId, userId, happensDate, new Photo(new byte[] {0x1}), 8, 1, new GeoTag(11.0, 16.0),
                    new Comment("здравствуй!")),
                //will pass filter
                CreateEventWithAll(trackerId, userId, happensDate, new Photo(new byte[] {0x1}), 8, 7, new GeoTag(11.0, 16.0),
                    new Comment("здравствуй!")),
            };

            //act
            var filteredEvents = EventsFilter.Filter(events, _filters).ToList();

            //assert
            Assert.AreEqual(1, filteredEvents.Count);
        }

        [Test]
        public void GeoTagPassTroughFilters()
        {
            //arrange
            var time = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero);
            var trackerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var events = new List<Event>
            {
                CreateEventWithComment(trackerId, userId, time, "привет!"),
                CreateEventWithScale(trackerId, userId, time, 1.0),
                CreateEventWithFixTime(trackerId, userId, DateTimeOffset.Now),
                CreateEventWithRating(trackerId, userId, time, 1.0),
                CreateEventWithAll(trackerId, userId, time, new Photo(new byte[] {0x1}), 8, 5.6, new GeoTag(11.0, 16.0),
                    new Comment("здравствуй!")),
                CreateEvent(trackerId, userId, time),
            };

            //act
            var filteredEvents = EventsFilter.Filter(events, _filters).ToList();

            //assert
            Assert.AreEqual(1, filteredEvents.Count);
        }
    }
}