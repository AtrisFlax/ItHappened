using System;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;

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
        public void EventTrackerHasOnlyRatingEvents_CalculateSuccess()
        {
            //arrange 
            var tracker = CreateTracker();
            var (events, ratings) = CreateEventsWithRating(tracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new AverageRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<AverageRatingTrackerFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual(Math.Sqrt(ratings.Average()), fact.Priority);
            Assert.AreEqual(ratings.Average(), fact.AverageRating);
        }

        [Test]
        public void EventTrackerHasEventsWithRatingAndEventsWithoutRating_CalculateSuccess()
        {
            //arrange 
            var tracker = CreateTracker();
            var (events, ratings) = CreateEventsWithRating(tracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            var eventsWithoutRating = CreateEventsWithoutCustomization(tracker.Id, _rand.Next() % 10 + MinEventForCalculation);
            _eventRepository.AddRangeOfEvents(events);
            _eventRepository.AddRangeOfEvents(eventsWithoutRating);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new AverageRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<AverageRatingTrackerFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual(Math.Sqrt(ratings.Average()), fact.Priority);
            Assert.AreEqual(ratings.Average(), fact.AverageRating);
        }


        [Test]
        public void EventTrackerHasOneEvent_CalculateFailed()
        {
            //arrange 
            var tracker = new EventTracker(Guid.NewGuid(), Guid.NewGuid(), "Tracker name",
                new TrackerCustomizationSettings());
            var (events, _) = CreateEventsWithRating(tracker.Id, 1);
            _eventRepository.AddRangeOfEvents(events);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new AverageRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<AverageRatingTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void EventTrackerHasZeroEvents_CalculateFailed()
        {
            //arrange 
            var tracker = new EventTracker(Guid.NewGuid(), Guid.NewGuid(), "Tracker name",
                new TrackerCustomizationSettings());
            var (events, _) = CreateEventsWithRating(tracker.Id, 0);
            var allEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);

            //act 
            var fact = new AverageRatingCalculator().Calculate(allEvents, tracker)
                .ConvertTo<AverageRatingTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }
    }
}