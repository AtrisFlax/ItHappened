using System;
using System.Collections.Generic;
using ItHappend.Domain;
using ItHappend.Domain.EventCustomization;
using ItHappend.Domain.Statistics.SingleTrackerCalculator;
using ItHappend.Domain.Statistics.StatisticsFacts;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappend.UnitTests.StatisticsCalculatorsTests
{
    public class WorstEventTests
    {
        private const int InitialEventsNumber = 9; 
        private Guid _creatorId;
        private List<Event> _events;
        private EventTracker _eventTracker;
        private WorstEventCalculator _worstEventCalculator;
        [SetUp]
        public void Init()
        {
            _creatorId = Guid.NewGuid();
            _events = CreateEvents(_creatorId,InitialEventsNumber);
            _eventTracker = CreateEventTracker(_creatorId,_events);
            _worstEventCalculator = new WorstEventCalculator();
        }

        [Test]
        public void CalculateLessThanRequiredNumberEvents_ReturnNone()
        {
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            var expected = Option<ISingleTrackerStatisticsFact>.None;
            
            var actual = _worstEventCalculator.Calculate(_eventTracker);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void CalculateWithoutOldEnoughEvent_ReturnNone()
        {
            _events.Add(CreateEvent(_creatorId));
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            var expected = Option<ISingleTrackerStatisticsFact>.None;
            
            var actual = _worstEventCalculator.Calculate(_eventTracker);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void CalculateWhenWorstEventHappenedLessThanWeekAgo_ReturnNone()
        {
            _events.Add(CreateEvent(_creatorId));
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(6);
            var expected = Option<ISingleTrackerStatisticsFact>.None;
            
            var actual = _worstEventCalculator.Calculate(_eventTracker);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void CalculateWithoutComment_ReturnsFact()
        {
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            _events.Add(CreateEvent(_creatorId));
            _events[9].Rating = 1;
            var expected = string.Empty;
        
            var res = _worstEventCalculator.Calculate(_eventTracker);

            Assert.Throws<NullReferenceException>(() =>
            {
                var comment = res.ValueUnsafe().Comment;
            });
        }
        
        // [Test]
        // public void CalculateWithAllConditionsGood_ReturnsFact()
        // {
        //     
        // }
        
        private static Event CreateEvent(Guid creatorId)
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