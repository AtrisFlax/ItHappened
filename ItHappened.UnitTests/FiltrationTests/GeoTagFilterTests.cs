using System;
using System.Collections.Generic;
using ItHappened.Domain;
using NUnit.Framework;

namespace ItHappened.UnitTests.FiltrationTests
{
    public class GeoTagFilterTests
    {
        private IEventsFilter _correctGeoTagFilter;
        private IEventsFilter _wrongGeoTagFilter;
        [SetUp]
        public void Init()
        {
            _correctGeoTagFilter = new GeoTagFilter("geotag",  new GeoTag(10, 15), new GeoTag(20, 25));
            _wrongGeoTagFilter = new GeoTagFilter("geotag", new GeoTag(15, 10), new GeoTag(0, 5));
        }
        
        [Test]
        public void FilterEventsWithCorrectRectangularCoordinates_ReturnFilteredCollection()
        {
            var events = CreateEvents(10);
            events[0].GeoTag = new GeoTag(11, 16);
            events[1].GeoTag = new GeoTag(10, 15);
            events[2].GeoTag = new GeoTag(20, 25);
            events[3].GeoTag = new GeoTag(10, 3);
            events[4].GeoTag = new GeoTag(0, 25);;
            const int expected = 3;
            
            var filteredEvents = _correctGeoTagFilter.Filter(events);
            
            Assert.AreEqual(expected, filteredEvents.Count);
        }
        
        [Test]
        public void FilterEventsWithWrongRectangularCoordinates_ReturnEmptyCollection()
        {
            var events = CreateEvents(10);
            events[0].GeoTag = new GeoTag(11, 16);
            events[1].GeoTag = new GeoTag(10, 15);
            events[2].GeoTag = new GeoTag(20, 25);
            events[3].GeoTag = new GeoTag(10, 3);
            events[4].GeoTag = new GeoTag(0, 25);;
            const int expected = 0;
            
            var filteredEvents = _wrongGeoTagFilter.Filter(events);
            
            Assert.AreEqual(expected, filteredEvents.Count);
        }
        
        private List<Event> CreateEvents(int quantity)
        {
            var events = new List<Event>();
            for (var i = 0; i < quantity; i++)
                events.Add(EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now, "tittle")
                    .WithGeoTag(new GeoTag(0, 0))
                    .Build());
            return events;
        }
    }
}