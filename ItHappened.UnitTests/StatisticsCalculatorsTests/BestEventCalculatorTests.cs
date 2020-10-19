using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class BestEventCalculatorTests
    {
        private const int InitialEventsNumber = 9;
        private BestEventCalculator _bestEventCalculator;
        private Guid _creatorId;
        private List<Event> _events;
        private EventTracker _eventTracker;

        [SetUp]
        public void Init()
        {
            _creatorId = Guid.NewGuid();
            _events = CreateEvents(_creatorId, InitialEventsNumber);
            _eventTracker = CreateEventTracker(_events);
            _bestEventCalculator = new BestEventCalculator();
        }

        [Test]
        public void CalculateLessThanRequiredNumberEvents_ReturnNone()
        {
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            var actual = _bestEventCalculator.Calculate(_eventTracker).ConvertTo<BestEventFact>();
            Assert.IsTrue(actual.IsNone);
        }

        [Test]
        public void CalculateWithoutOldEnoughEvent_ReturnNone()
        {
            _events.Add(CreateEventWithoutComment(_creatorId));
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            var actual = _bestEventCalculator.Calculate(_eventTracker).ConvertTo<BestEventFact>();
            Assert.IsTrue(actual.IsNone);
        }

        [Test]
        public void CalculateWhenBestEventHappenedLessThanWeekAgo_ReturnNone()
        {
            _events.Add(CreateEventWithoutComment(_creatorId));
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(6);
            var actual = _bestEventCalculator.Calculate(_eventTracker).ConvertTo<BestEventFact>();
            Assert.IsTrue(actual.IsNone);
        }

        [Test]
        public void CalculateGoodCase_ReturnsFact()
        {
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events.Add(CreateEventWithoutComment(_creatorId));
            _events[9].Rating = 9;
            _events[9].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            
            var actual = _bestEventCalculator.Calculate(_eventTracker).ConvertTo<BestEventFact>();
            Assert.IsTrue(actual.IsSome);
            //TODO check expected fields
        }

        private Event CreateEventWithoutComment(Guid creatorId)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), creatorId, _eventTracker.Id, DateTimeOffset.Now, "tittle")
                .WithRating(5)
                .Build();
        }

        private List<Event> CreateEvents(Guid creatorId, int quantity)
        {
            var events = new List<Event>();
            for (var i = 0; i < quantity; i++)
                events.Add(EventBuilder
                    .Event(Guid.NewGuid(), creatorId, _eventTracker.Id,DateTimeOffset.Now, "tittle")
                    .WithComment("comment")
                    .WithRating(5)
                    .Build());

            return events;
        }

        private EventTracker CreateEventTracker(IEnumerable<Event> eventList)
        {
            var tracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), _eventTracker.Id,  "tracker")
                .Build();
            foreach (var @event in eventList)
            {
                tracker.AddEvent(@event);
            }
            return tracker;
        }
    }
}