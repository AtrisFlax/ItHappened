using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;

namespace ItHappened.UnitTests.FiltrationTests
{
    public class GeoTagFilterTests
    {
        private IEventsFilter _correctGeoTagFilter;
        private IEventsFilter _wrongGeoTagFilter;

        [SetUp]
        public void Init()
        {
            _correctGeoTagFilter = new GeoTagFilter("geotag", new GeoTag(10, 15), new GeoTag(20, 25));
            _wrongGeoTagFilter = new GeoTagFilter("geotag", new GeoTag(15, 10), new GeoTag(0, 5));
        }

        [Test]
        public void FilterEventsWithCorrectRectangularCoordinates_ReturnFilteredCollection()
        {
            var events = new List<Event>
            {
                CreateEventWithGeoTag(Guid.NewGuid(), Guid.NewGuid(), new GeoTag(11.0, 16.0)),
                CreateEventWithGeoTag(Guid.NewGuid(), Guid.NewGuid(), new GeoTag(10.0, 15.0)),
                CreateEventWithGeoTag(Guid.NewGuid(), Guid.NewGuid(), new GeoTag(20.0, 25.0)),
                CreateEventWithGeoTag(Guid.NewGuid(), Guid.NewGuid(), new GeoTag(10.0, 3.0)),
                CreateEventWithGeoTag(Guid.NewGuid(), Guid.NewGuid(), new GeoTag(0.0, 25.0))
            };

            const int expected = 3;

            var filteredEvents = _correctGeoTagFilter.Filter(events);

            Assert.AreEqual(expected, filteredEvents.Count());
        }

        [Test]
        public void FilterEventsWithWrongRectangularCoordinates_ReturnEmptyCollection()
        {
            var events = new List<Event>
            {
                CreateEventWithGeoTag(Guid.NewGuid(), Guid.NewGuid(), new GeoTag(11, 16)),
                CreateEventWithGeoTag(Guid.NewGuid(), Guid.NewGuid(), new GeoTag(10, 15)),
                CreateEventWithGeoTag(Guid.NewGuid(), Guid.NewGuid(), new GeoTag(20, 25)),
                CreateEventWithGeoTag(Guid.NewGuid(), Guid.NewGuid(), new GeoTag(10, 3)),
                CreateEventWithGeoTag(Guid.NewGuid(), Guid.NewGuid(), new GeoTag(0, 25))
            };

            const int expected = 0;

            var filteredEvents = _wrongGeoTagFilter.Filter(events);

            Assert.AreEqual(expected, filteredEvents.Count());
        }
    }
}