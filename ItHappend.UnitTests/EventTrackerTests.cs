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
            var eventList = new List<Event>();
            var initEvent = EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now, "Title",
                    0.5)
                .Build();
            eventList.Add(initEvent);
            var eventForAdding = EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now, "Added Title",
                    0.5)
                .Build();


            //act
            var eventTracker = CreateEventTracker(false, eventList);
            eventTracker.AddEvent(eventForAdding);
            
            //assert
            Assert.That(eventTracker.Events, Is.EquivalentTo(new List<Event> {initEvent, eventForAdding}));
        }
        
        public void RemoveEventFromTracker()
        {
            //arrange
            var eventList = new List<Event>();
            var initEvent = EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now, "Title",
                    0.5)
                .Build();
            eventList.Add(initEvent);
            var eventForAdding = EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.Now, "Added Title",
                    0.5)
                .Build();


            //act
            var eventTracker = CreateEventTracker(false, eventList);
            eventTracker.RemoveEvent(eventForAdding);
            
            //assert
            Assert.That(eventTracker.Events, Is.EquivalentTo(new List<Event> {initEvent, eventForAdding}));
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