using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;


namespace ItHappened.UnitTests.FiltrationTests
{
    public class RatingFilterTests
    {
        private IEventsFilter _ratingFilter;

        [SetUp]
        public void Init()
        {
            _ratingFilter = new RatingFilter("rating", 5.5, 8.6);
        }

        [Test]
        public void FilterEvents_ReturnFilteredCollection()
        {
            var events = new List<Event>
            {
                CreateEventWithRating(Guid.NewGuid(), Guid.NewGuid(), 5.5),
                CreateEventWithRating(Guid.NewGuid(), Guid.NewGuid(), 8.6),
                CreateEventWithRating(Guid.NewGuid(), Guid.NewGuid(), 6),
                CreateEventWithRating(Guid.NewGuid(), Guid.NewGuid(), 5.499),
                CreateEventWithRating(Guid.NewGuid(), Guid.NewGuid(), 8.601)
            };
            const int expected = 3;

            var filteredEvents = _ratingFilter.Filter(events);

            Assert.AreEqual(expected, filteredEvents.Count());
        }

        [Test]
        public void FilterEventsWithoutRating_ReturnEmptyCollection()
        {
            var events = CreateEvents(Guid.NewGuid(), Guid.NewGuid(), 10);

            const int expected = 0;

            var filteredEvents = _ratingFilter.Filter(events);

            Assert.AreEqual(expected, filteredEvents.Count());
        }
    }
}