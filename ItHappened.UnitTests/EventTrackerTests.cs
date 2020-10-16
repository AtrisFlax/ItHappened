using System;
using System.Collections.Generic;
using ItHappened.Domain;
using NUnit.Framework;

namespace ItHappened.UnitTests
{
    public class EventTrackerTests
    {
        [Test]
        public void AddEventToTracker()
        {
            //arrange
            var initEvent = CreateEvent("Title");
            var eventForAdding = CreateEvent("Added Title");
            var eventList = new List<Event> {initEvent};

            //act
            var eventTracker = CreateEventTracker(eventList);
            eventTracker.AddEvent(eventForAdding);

            //assert
            Assert.That(eventTracker.Events, Is.EquivalentTo(new List<Event> {initEvent, eventForAdding}));
        }
        
        [Test]
        public void RemoveEventFromTracker()
        {
            //arrange
            var firstEvent = CreateEvent("Should left");
            var secondEvent = CreateEvent("For remove");
            var eventList = new List<Event>{firstEvent, secondEvent};
            
            //act
            var eventTracker = CreateEventTracker(eventList);
            eventTracker.RemoveEvent(secondEvent);

            //assert
            Assert.That(eventTracker.Events, Is.EquivalentTo(new List<Event> {firstEvent}));
        }

        [Test]
        public void FilterEventsByTimeSpan_ShouldReturnNoEvents()
        {
            //arrange
            var eventList = new List<Event>();
            var from = new DateTimeOffset(DateTime.Now);
            var to = new DateTimeOffset(DateTime.Now + TimeSpan.FromDays(1));
            var expected = 0;
            
            //act
            var eventTracker = CreateEventTracker(eventList);
            var filteredEvents = eventTracker.FilterEventsByTimeSpan(from, to);
            var actual = filteredEvents.Count;
            
            //assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void FilterEventsByTimeSpan_ShouldFilterEvents()
        {
            //arrange
            var firstEvent = CreateEvent("First");
            firstEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero).ToLocalTime();
            var secondEvent = CreateEvent("Second");
            secondEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 18, 00, 59, TimeSpan.Zero).ToLocalTime();
            var thirdEvent = CreateEvent("Third");
            thirdEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 17, 59, 59, TimeSpan.Zero).ToLocalTime();
            var eventList = new List<Event> {firstEvent, secondEvent, thirdEvent};
            var from = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero);
            var to = new DateTimeOffset(2020, 10, 14, 18, 01, 00, TimeSpan.Zero);
            var expected = 2;
            
            //act
            var eventTracker = CreateEventTracker(eventList);
            var filteredEvents = eventTracker.FilterEventsByTimeSpan(from, to);
            var actual = filteredEvents.Count;
            
            //assert
            Assert.AreEqual(expected, actual);
        }
        
        private static Event CreateEvent(string tittle)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now, tittle, 0.5)
                .Build();
        }

        private static EventTracker CreateEventTracker(List<Event> eventList)
        {
            return new EventTracker(Guid.NewGuid(),
                "name1",
                eventList,
                Guid.NewGuid()
            );
        }
    }
}