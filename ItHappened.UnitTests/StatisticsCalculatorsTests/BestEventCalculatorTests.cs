using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class BestEventCalculatorTests
    {
        private const int InitialEventsNumber = 9;
        private BestRatingEventCalculator _bestRatingEventCalculator;
        private Guid _creatorId;
        private List<Event> _events;
        private EventTracker _eventTracker;
        private IEventRepository _eventRepository;
        
        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
            _creatorId = Guid.NewGuid();
            _eventTracker = CreateEventTracker();
            _events = CreateEvents(_creatorId, InitialEventsNumber);
            _eventRepository.AddRangeOfEvents(_events);
            _bestRatingEventCalculator = new BestRatingEventCalculator(_eventRepository);
        }

        [Test]
        public void CalculateLessThanRequiredNumberEvents_ReturnNone()
        {
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            
            var actual = _bestRatingEventCalculator.Calculate(_eventTracker);
            
            Assert.IsTrue(actual.IsNone);
        }

        [Test]
        public void CalculateWithoutOldEnoughEvent_ReturnNone()
        {
            _eventRepository.AddEvent(CreateEventWithoutComment(_creatorId));
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            
            var actual = _bestRatingEventCalculator.Calculate(_eventTracker);
            
            Assert.IsTrue(actual.IsNone);
        }

        [Test]
        public void CalculateWhenBestEventHappenedLessThanWeekAgo_ReturnNone()
        {
            _eventRepository.AddEvent(CreateEventWithoutComment(_creatorId));
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            _events[1].Rating = 1;
            _events[1].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(6);
            
            var actual = _bestRatingEventCalculator.Calculate(_eventTracker);
            
            Assert.IsTrue(actual.IsNone);
        }

        [Test]
        public void CalculateGoodCase_ReturnsFact()
        {
            _events[0].HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(91);
            var event10 = CreateEventWithoutComment(_creatorId);
            event10.Comment = new Comment("123");
            event10.Rating = 9;
            event10.HappensDate = DateTimeOffset.Now - TimeSpan.FromDays(8);
            _eventRepository.AddEvent(event10);

            var optionalBestEventFact = _bestRatingEventCalculator.Calculate(_eventTracker).ConvertTo<BestEventTrackerFact>();
            var bestEventFact = optionalBestEventFact.ValueUnsafe();
            
            Assert.IsTrue(optionalBestEventFact.IsSome);
            Assert.AreEqual(bestEventFact.Priority, event10.Rating.ValueUnsafe());
            Assert.AreEqual(bestEventFact.EventReference, event10);
            Assert.AreEqual(bestEventFact.HappensDate, event10.HappensDate);
            Assert.AreEqual(bestEventFact.Comment.Text, event10.Comment.ValueUnsafe().Text);
            Assert.AreEqual(bestEventFact.FactName, "Лучшее событие");
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
            {
                events.Add(EventBuilder
                    .Event(Guid.NewGuid(), creatorId, _eventTracker.Id, DateTimeOffset.Now, "tittle")
                    .WithComment("comment")
                    .WithRating(5)
                    .Build());
            }
            return events;
        }

        private EventTracker CreateEventTracker()
        {
            var tracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(),  "tracker")
                .Build();
            return tracker;
        }
    }
}