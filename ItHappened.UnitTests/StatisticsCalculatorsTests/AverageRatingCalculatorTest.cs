using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class AverageRatingCalculatorTest
    {
        private IEventRepository _eventRepository;
        
        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
        }
        
        [Test]
        public void EventTrackerHasTwoRatingAndEvents_CalculateSuccess()
        {
            //arrange 
            var ratings = new List<double> {2.0, 5.0};
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .WithRating()
                .Build();
            var events = CreateTwoEvents(eventTracker.Id,ratings);
            _eventRepository.AddRangeOfEvents(events);
                //act 
            var fact = new AverageRatingCalculator(_eventRepository).Calculate(eventTracker).ConvertTo<AverageRatingFact>();
            //assert 
            Assert.True(fact.IsSome);
            fact.Do(f =>
            {
                Assert.AreEqual(Math.Sqrt(ratings.Average()), f.Priority);
                Assert.AreEqual(ratings.Average(), f.AverageRating);
            });
        }

        [Test]
        public void EventTrackerHasNoRationCustomization_CalculateFailed()
        {
            //arrange 
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            var events = CreateTwoEvents(eventTracker.Id);
            _eventRepository.AddRangeOfEvents(events);
            //act 
            var fact = new AverageRatingCalculator(_eventRepository).Calculate(eventTracker).ConvertTo<AverageRatingFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasOneEvent_CalculateFailed()
        {
            //arrange 
            var ratings = new List<double> {2.0};
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .WithRating()
                .Build();
            var @event = EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), eventTracker.Id, DateTimeOffset.UtcNow, "Event1").WithRating(ratings[0])
                .Build();
            _eventRepository.AddEvent(@event);
            //act 
            var fact = new AverageRatingCalculator(_eventRepository).Calculate(eventTracker).ConvertTo<AverageRatingFact>();

            //assert 
            Assert.True(fact.IsNone);
        }


        [Test]
        public void SomeEventHasNoCustomizationRating_CalculateFailed()
        {
            //arrange 
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            var events = CreateTwoEventOneOfThemWithoutRating(eventTracker.Id);
            _eventRepository.AddRangeOfEvents(events);
            //act 
            var fact = new AverageRatingCalculator(_eventRepository).Calculate(eventTracker).ConvertTo<AverageRatingFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        private static IEnumerable<Event> CreateTwoEventOneOfThemWithoutRating(Guid trackerId)
        {
            var ratings = new List<double> {2.0};
            return new List<Event>
            {
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.UtcNow, "Event1")
                    .Build(),
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.UtcNow, "Event2")
                    .WithRating(ratings[0])
                    .Build()
            };
        }

        private static IEnumerable<Event> CreateTwoEvents(Guid trackerId)
        {
            var ratings = new List<double> {2.0, 5.0};
            return new List<Event>
            {
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId,DateTimeOffset.UtcNow, "Event1").WithRating(ratings[0])
                    .Build(),
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId,DateTimeOffset.UtcNow, "Event1").WithRating(ratings[1])
                    .Build()
            };
        }

        private static IEnumerable<Event> CreateTwoEvents(Guid trackerId, IList<double> ratings)
        {
            return new List<Event>
            {
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId,DateTimeOffset.UtcNow, "Event1").WithRating(ratings[0])
                    .Build(),
                EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId,DateTimeOffset.UtcNow, "Event1").WithRating(ratings[1])
                    .Build()
            };
        }
    }
}