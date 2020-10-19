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
            var initEvent = CreateEvent(title);
            var eventForAdding = CreateEvent("Added Title");
            var eventList = new List<Event> {initEvent};
            var eventTrackerId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();

            //act
            var eventTracker = CreateEventTracker(creatorId, eventTrackerId, eventList);
            eventTracker.AddEvent(eventForAdding);

            //assert
            Assert.That(eventTracker.Events, Is.EquivalentTo(new List<Event> {initEvent, eventForAdding}));
            Assert.AreEqual(title, eventTracker.Name);
            Assert.AreEqual(eventTrackerId, eventTracker.TrackerId);
        }

        [Test]
        public void RemoveEventFromTracker()
        {
            //arrange
            var firstEvent = CreateEvent("Should left");
            var secondEvent = CreateEvent("For remove");
            var eventList = new List<Event> {firstEvent, secondEvent};
            var eventTrackerId = Guid.NewGuid();
            var creatorId = Guid.NewGuid();

            //act
            var eventTracker = CreateEventTracker(creatorId, eventTrackerId, eventList);
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
            var firstEvent = CreateEvent("First");
            var secondEvent = CreateEvent("Second");
            var thirdEvent = CreateEvent("Third");
            firstEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero).ToLocalTime();
            secondEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 18, 00, 59, TimeSpan.Zero).ToLocalTime();
            thirdEvent.HappensDate = new DateTimeOffset(2020, 10, 14, 17, 59, 59, TimeSpan.Zero).ToLocalTime();
            var eventList = new List<Event> {firstEvent, secondEvent, thirdEvent};
            var from = new DateTimeOffset(2020, 10, 14, 18, 00, 00, TimeSpan.Zero);
            var to = new DateTimeOffset(2020, 10, 14, 18, 01, 00, TimeSpan.Zero);
            const int expected = 2;
            var creatorId = Guid.NewGuid();
            var trackerId = Guid.NewGuid();

            //act
            var eventTracker = CreateEventTracker(creatorId, trackerId, eventList);
            var filteredEvents = eventTracker.FilterEventsByTimeSpan(from, to);
            var actual = filteredEvents.Count;

            //assert
            Assert.AreEqual(expected, actual);
        }

        private static Event CreateEvent(string tittle)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now, tittle)
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