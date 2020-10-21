using System;
using System.Collections.Generic;
using ItHappened.Domain;
using NUnit.Framework;

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
            _dateTimeFilter = new DateTimeFilter("DateTime",  from, to);
        }
        
        [Test]
        public void FilterEvents_ReturnFilteredCollection()
        {
            var firstEvent = CreateEvent("First");
            firstEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero).ToLocalTime();
            var secondEvent = CreateEvent("Second");
            secondEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 18, 00, 59, TimeSpan.Zero).ToLocalTime();
            var thirdEvent = CreateEvent("Third");
            thirdEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 17, 59, 59, TimeSpan.Zero).ToLocalTime();
            var eventList = new List<Event> {firstEvent, secondEvent, thirdEvent};
            const int expected = 2;
            
            var filteredEvents = _dateTimeFilter.Filter(eventList);
            var actual = filteredEvents.Count;
            
            Assert.AreEqual(expected, actual);
        }
        
        private static Event CreateEvent(string tittle)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),DateTimeOffset.Now, "tittle")
                .Build();
        }
    }
}