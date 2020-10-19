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
            const string title = "Title";
            var eventTrackerId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var eventList = new List<Event>();
            var eventTracker = CreateEventTracker(creatorId, eventTrackerId, eventList);
            var initEvent = CreateEvent(eventTracker.Id, title);
            var eventForAdding = CreateEvent(eventTracker.Id,"Added Title");
            eventTracker.AddEvent(initEvent);

            //act
            
            eventTracker.AddEvent(eventForAdding);

            //assert
            Assert.That(eventTracker.Events, Is.EquivalentTo(new List<Event> {initEvent, eventForAdding}));
            Assert.AreEqual(title, eventTracker.Name);
            Assert.AreEqual(eventTrackerId, eventTracker.Id);
        }

        [Test]
        public void RemoveEventFromTracker()
        {
            //arrange
            var eventTrackerId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();
            var firstEvent = CreateEvent(eventTrackerId,"Should left");
            var secondEvent = CreateEvent(eventTrackerId, "For remove");
            var eventList = new List<Event> {firstEvent, secondEvent};
            var eventTracker = CreateEventTracker(creatorId, eventTrackerId, eventList);
            //act
            
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
            var eventTrackerId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();

            //act
            var eventTracker = CreateEventTracker(creatorId, eventTrackerId, eventList);
            var filteredEvents = eventTracker.FilterEventsByTimeSpan(from, to);
            var actual = filteredEvents.Count;

            //assert
            Assert.AreEqual(0, actual);
        }

        [Test]
        public void FilterEventsByTimeSpan_ShouldFilterEvents()
        {
            //arrange
            var creatorId = Guid.NewGuid();
            var trackerId = Guid.NewGuid();
            var firstEvent = CreateEvent(trackerId,"First");
            var secondEvent = CreateEvent(trackerId, "Second");
            var thirdEvent = CreateEvent(trackerId, "Third");
            var eventList = new List<Event> {firstEvent, secondEvent, thirdEvent};
            var eventTracker = CreateEventTracker(creatorId, trackerId, eventList);
            firstEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero).ToLocalTime();
            secondEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 18, 00, 59, TimeSpan.Zero).ToLocalTime();
            thirdEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 17, 59, 59, TimeSpan.Zero).ToLocalTime();
            var from = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero);
            var to = new DateTimeOffset(2020, 10, 14, 18, 01, 00, TimeSpan.Zero);
            const int expected = 2;

            //act
            var filteredEvents = eventTracker.FilterEventsByTimeSpan(from, to);
            var actual = filteredEvents.Count;

            //assert
            Assert.AreEqual(expected, actual);
        }

        private static Event CreateEvent(Guid trackerId, string tittle)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.Now, tittle)
                .Build();
        }

        private static EventTracker CreateEventTracker(Guid creatorId, Guid trackerId, List<Event> eventList)
        {
            var tracker = EventTrackerBuilder
                .Tracker(creatorId, creatorId, "tracker")
                .Build();
            foreach (var @event in eventList)
            {
                tracker.AddEvent(@event);
            }

            return tracker;
        }
    }
}