using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;

namespace ItHappened.UnitTests.FiltrationTests
{
    public class ScaleFilterTests
    {
        private IEventsFilter _scaleFilter;

        [SetUp]
        public void Init()
        {
            _scaleFilter = new ScaleFilter("scale", 5.5, 8.6);
        }

        [Test]
        public void FilterEvents_ReturnFilteredCollection()
        {
            var events = new List<Event>
            {
                CreateEventWithScale(Guid.NewGuid(), Guid.NewGuid(), 5.5),
                CreateEventWithScale(Guid.NewGuid(), Guid.NewGuid(), 8.6),
                CreateEventWithScale(Guid.NewGuid(), Guid.NewGuid(), 6),
                CreateEventWithScale(Guid.NewGuid(), Guid.NewGuid(), 5.499),
                CreateEventWithScale(Guid.NewGuid(), Guid.NewGuid(), -5.499),
                CreateEventWithScale(Guid.NewGuid(), Guid.NewGuid(), -0),
                CreateEventWithScale(Guid.NewGuid(), Guid.NewGuid(), 8.601)
            };

            const int expected = 3;

            var filteredEvents = _scaleFilter.Filter(events);

            Assert.AreEqual(expected, filteredEvents.Count());
        }
    }
}