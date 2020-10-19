using System;
using System.Collections.Generic;
using ItHappend.Domain.Statistics;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class WorstEventCalculatorTests
    {
        private const int InitialEventsNumber = 9;
        private Guid _creatorId;
        private List<Event> _events;
        private EventTracker _eventTracker;
        private WorstEventCalculator _worstEventCalculator;
        private IEventRepository _eventRepository;
        
        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
            _creatorId = Guid.NewGuid();
            _events = CreateEvents(_creatorId, InitialEventsNumber);
            _eventTracker = CreateEventTracker();
            _worstEventCalculator = new WorstEventCalculator(_eventRepository);
        }

        [Test]
        public void CalculateLessThanRequiredNumberEvents_ReturnNone()
        {
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            var expected = Option<IStatisticsFact>.None;

            var actual = _worstEventCalculator.Calculate(_eventTracker);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateWithoutOldEnoughEvent_ReturnNone()
        {
            _events.Add(CreateEventWithoutComment(_creatorId));
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            var expected = Option<IStatisticsFact>.None;

            var actual = _worstEventCalculator.Calculate(_eventTracker);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateWhenWorstEventHappenedLessThanWeekAgo_ReturnNone()
        {
            _events.Add(CreateEventWithoutComment(_creatorId));
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(6);
            var expected = Option<IStatisticsFact>.None;

            var actual = _worstEventCalculator.Calculate(_eventTracker);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateGoodCase_ReturnsFact()
        {
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events.Add(CreateEventWithoutComment(_creatorId));
            _events[9].Rating = 1;
            _events[9].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);

            var worstEvent = _worstEventCalculator.Calculate(_eventTracker);

            Assert.True(worstEvent.IsSome);
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
                    .Event(Guid.NewGuid(), creatorId, _eventTracker.Id, DateTimeOffset.Now, "tittle")
                    .WithComment("comment")
                    .WithRating(5)
                    .Build());

            return events;
        }

        private EventTracker CreateEventTracker()
        {
            var tracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), _eventTracker.Id, "tracker")
                .Build();

            return tracker;
        }
    }
}