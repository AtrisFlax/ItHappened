using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;


namespace ItHappened.UnitTests.FiltrationTests
{
    public class DateTimeFilterTests
    {
        private IEventsFilter _dateTimeFilter;

        [SetUp]
        public void Init()
        {
            var from = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero);
            var to = new DateTimeOffset(2020, 10, 14, 18, 01, 00, TimeSpan.Zero);
            _dateTimeFilter = new DateTimeFilter("DateTime", from, to);
        }

        [Test]
        public void FilterEvents_ReturnFilteredCollection()
        {
            var firstEvent = CreateEventWithFixTime(Guid.NewGuid(), Guid.NewGuid(),
                new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero).ToLocalTime());
            var secondEvent = CreateEventWithFixTime(Guid.NewGuid(), Guid.NewGuid(),
                new DateTimeOffset(2020, 10, 14, 18, 00, 59, TimeSpan.Zero).ToLocalTime());
            var thirdEvent = CreateEventWithFixTime(Guid.NewGuid(), Guid.NewGuid(),
                new DateTimeOffset(2020, 10, 14, 17, 59, 59, TimeSpan.Zero).ToLocalTime());
            var eventList = new List<Event> {
            firstEvent, 
            secondEvent, 
            thirdEvent};

            var filteredEvents = _dateTimeFilter.Filter(eventList);
            var actual = filteredEvents.Count();

            Assert.AreEqual(2, actual);
        }
    }
}