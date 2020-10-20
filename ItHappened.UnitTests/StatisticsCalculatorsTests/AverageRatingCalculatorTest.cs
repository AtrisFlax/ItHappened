using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class AverageRatingCalculatorTest
    {
        private const int MinEventForCalculation = 2;
        private IEventRepository _eventRepository;
        private static Random _rand;

        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
            _rand = new Random();
        }

        [Test]
        public void EventTrackerHasTwoRatingAndEvents_CalculateSuccess()
        {
            //arrange 
            var eventTracker = CreateTrackerWithRating();
            var (events, ratings) = CreateEventsWithRating(eventTracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);
            
            //act 
            var fact = new AverageRatingCalculator(_eventRepository).Calculate(eventTracker)
                .ConvertTo<AverageRatingFact>().ValueUnsafe();
            
            //assert 
            Assert.AreEqual(Math.Sqrt(ratings.Average()), fact.Priority);
            Assert.AreEqual(ratings.Average(), fact.AverageRating);
        }

        [Test]
        public void EventTrackerHasNoRationCustomization_CalculateFailed()
        {
            //arrange 
            var eventTracker = CreateTrackerWithoutRating();
            var (events, _) = CreateEventsWithRating(eventTracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);
            
            //act 
            var fact = new AverageRatingCalculator(_eventRepository).Calculate(eventTracker)
                .ConvertTo<AverageRatingFact>();
            
            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasOneEvent_CalculateFailed()
        {
            //arrange 
            var eventTracker = CreateTrackerWithRating();
            var (events, _) = CreateEventsWithRating(eventTracker.Id, 1);
            _eventRepository.AddRangeOfEvents(events);
            
            //act 
            var fact = new AverageRatingCalculator(_eventRepository).Calculate(eventTracker)
                .ConvertTo<AverageRatingFact>();
            
            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasZeroEvents_CalculateFailed()
        {
            //arrange 
            var eventTracker = CreateTrackerWithRating();
            
            //act 
            var fact = new AverageRatingCalculator(_eventRepository).Calculate(eventTracker)
                .ConvertTo<AverageRatingFact>();
            
            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasNoCustomizationRating_CalculateFailed()
        {
            //arrange 
            var eventTracker = CreateTrackerWithoutRating();
            var (events, _) = CreateEventsWithRating(eventTracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);
            
            //act 
            var fact = new AverageRatingCalculator(_eventRepository).Calculate(eventTracker)
                .ConvertTo<AverageRatingFact>();
            
            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasCustomizationRatingEventsHasNotCustomization_CalculateFailed()
        {
            //arrange 
            var eventTracker = CreateTrackerWithRating();
            var events = CreateEventsWithoutRating(eventTracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);
            
            //act 
            var fact = new AverageRatingCalculator(_eventRepository).Calculate(eventTracker)
                .ConvertTo<AverageRatingFact>();
            
            //assert 
            Assert.True(fact.IsNone);
        }

        private static EventTracker CreateTrackerWithRating()
        {
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .WithRating()
                .Build();
            return eventTracker;
        }

        private static EventTracker CreateTrackerWithoutRating()
        {
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            return eventTracker;
        }

        private static (IEnumerable<Event> events, List<double> Rating) CreateEventsWithRating(Guid trackerId, int num)
        {
            var ratings = CreateRandomRatings(num);
            var events = ratings.Select(t => CreateEventWithRating(trackerId, t)).ToList();
            return (events, ratings);
        }

        private static IEnumerable<Event> CreateEventsWithoutRating(Guid trackerId, int num)
        {
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(CreateEventWithoutRating(trackerId));
            }
            return events;
        }

        private static List<double> CreateRandomRatings(int num)
        {
            var ratings = new List<double>();
            for (var i = 0; i < num; i++)
            {
                ratings.Add(_rand.NextDouble());
            }
            return ratings;
        }

        private static Event CreateEventWithRating(Guid trackerId, double rating)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.UtcNow, $"Event with rating {rating}")
                .WithRating(rating)
                .Build();
        }

        private static Event CreateEventWithoutRating(Guid trackerId)
        {
            return EventBuilder
                .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.UtcNow, "Event without rating")
                .Build();
        }
    }
}