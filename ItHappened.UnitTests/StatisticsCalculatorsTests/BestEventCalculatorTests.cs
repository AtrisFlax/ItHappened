using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class BestEventCalculatorTests
    {
        private const int InitialEventsNumber = 9; 
        private Guid _creatorId;
        private List<Event> _events;
        private EventTracker _eventTracker;
        private BestEventCalculator _bestEventCalculator;
        
        [SetUp]
        public void Init()
        {
            _creatorId = Guid.NewGuid();
            _events = CreateEvents(_creatorId,InitialEventsNumber);
            _eventTracker = CreateEventTracker(_creatorId,_events);
            _bestEventCalculator = new BestEventCalculator();
        }
        
        [Test]
        public void CalculateLessThanRequiredNumberEvents_ReturnNone()
        {
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            var expected = Option<ISingleTrackerStatisticsFact>.None;
            
            var actual = _bestEventCalculator.Calculate(_eventTracker);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void CalculateWithoutOldEnoughEvent_ReturnNone()
        {
            _events.Add(CreateEventWithoutComment(_creatorId));
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            var expected = Option<ISingleTrackerStatisticsFact>.None;
            
            var actual = _bestEventCalculator.Calculate(_eventTracker);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void CalculateWhenBestEventHappenedLessThanWeekAgo_ReturnNone()
        {
            _events.Add(CreateEventWithoutComment(_creatorId));
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(6);
            var expected = Option<ISingleTrackerStatisticsFact>.None;
            
            var actual = _bestEventCalculator.Calculate(_eventTracker);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void CalculateGoodCase_ReturnsFact()
        {
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events.Add(CreateEventWithoutComment(_creatorId));
            _events[9].Rating = 9;
            _events[9].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
        
            var bestEvent = _bestEventCalculator.Calculate(_eventTracker);
        
            Assert.True(bestEvent.IsSome);
        }

        private Event CreateEventWithoutComment(Guid creatorId)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), creatorId, DateTimeOffset.Now, "tittle")
                .WithRating(5)
                .Build();
        }
        
        private List<Event> CreateEvents(Guid creatorId, int quantity)
        {
            var events = new List<Event>();
            for (var i = 0; i < quantity; i++)
            {
                events.Add(EventBuilder
                    .Event(Guid.NewGuid(), creatorId, DateTimeOffset.Now, "tittle")
                    .WithComment("comment")
                    .WithRating(5)
                    .Build());
            }

            return events;
        }

        private EventTracker CreateEventTracker(Guid creatorId, List<Event> eventList)
        {
            return new EventTracker(Guid.NewGuid(),
                "tracker",
                eventList,
                creatorId
            );
        }
    }
}