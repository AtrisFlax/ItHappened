using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ItHappend.Domain;
using NUnit.Framework;

namespace ItHappend.UnitTests
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
            var eventTracker = CreateEventTracker(false, eventList);
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
            var eventTracker = CreateEventTracker(false, eventList);
            eventTracker.RemoveEvent(secondEvent);

            //assert
            Assert.That(eventTracker.Events, Is.EquivalentTo(new List<Event> {firstEvent}));
        }

        private static Event CreateEvent(string tittle)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now, tittle, 0.5)
                .Build();
        }

        private static EventTracker CreateEventTracker(bool empty, List<Event> eventList)
        {
            return new EventTracker(Guid.NewGuid(),
                "name1",
                eventList,
                Guid.NewGuid()
            );
        }
    }
}