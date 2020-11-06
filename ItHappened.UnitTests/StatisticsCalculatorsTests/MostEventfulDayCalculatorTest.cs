using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure;
using ItHappened.Infrastructure.InMemoryRepositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class MostEventfulDayCalculatorTest
    {
        private IEventRepository _eventRepository;
        private DateTimeOffset _now;
        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
            _now = DateTimeOffset.UtcNow;
        }

        [Test]
        public void FindInTwoTrackerWithFactMostEventfulDay_CalculateSuccess()
        {
            //arrange
            const int expectedDaysAgo = -15;
            var expectedDate = _now.AddDays(-expectedDaysAgo);
            var userId = Guid.NewGuid();
            var tracker1 = CreateTrackerWithDefaultCustomization(userId);
            var tracker2 = CreateTrackerWithDefaultCustomization(userId);
            var eventsTracker1 = CreateEventsEveryDayByDayInPast(tracker1.Id, userId, 10, _now);
            var eventsTracker2 = CreateEventsEveryDayByDayInPast(tracker1.Id, userId, 10, _now);
            var eventsExtraEvents1 =
                CreateEventsEveryDaysAgo(tracker1.Id, userId, expectedDaysAgo, 70, _now);
            var eventsExtraEvents2 = CreateEventsEveryDaysAgo(tracker1.Id, userId, 9, 45, _now);
            var eventsExtraEvents3 = CreateEventsEveryDaysAgo(tracker2.Id, userId, 5, 20, _now);
            var eventsExtraEvents4 = CreateEventsEveryDaysAgo(tracker2.Id, userId, 40, 15, _now);
            _eventRepository.AddRangeOfEvents(eventsTracker1);
            _eventRepository.AddRangeOfEvents(eventsTracker2);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents1);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents2);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents3);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents4);
            var allEventsTracker1 = _eventRepository.LoadAllTrackerEvents(tracker1.Id);
            var allEventsTracker2 = _eventRepository.LoadAllTrackerEvents(tracker2.Id);
            var trackerWithItsEvents = new List<TrackerWithItsEvents>
            {
                new TrackerWithItsEvents(tracker1, allEventsTracker1),
                new TrackerWithItsEvents(tracker1, allEventsTracker2)
            };

            //act
            var fact = new MostEventfulDayCalculator()
                .Calculate(trackerWithItsEvents, _now)
                .ConvertTo<MostEventfulDayTrackersFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual("Самый насыщенный событиями день", fact.FactName);
            Assert.AreEqual($"Самый насыщенный событиями день был {expectedDate:d}. Тогда произошло 70 событий",
                fact.Description);
            Assert.AreEqual(105, fact.Priority);
            Assert.AreEqual(expectedDate, fact.DayWithLargestEventsCount);
            Assert.AreEqual(70, fact.EventsCount);
        }

        [Test]
        public void TrackerHaveZeroEvent_CalculateFailure()
        {
            //arrange
            var userId = Guid.NewGuid();
            var tracker = CreateTrackerWithDefaultCustomization(userId);
            var events = CreateEventsEveryDayByDayInPast(tracker.Id, userId, 0, _now);
            _eventRepository.AddRangeOfEvents(events);
            var allEventsTracker = _eventRepository.LoadAllTrackerEvents(tracker.Id);
            var trackerWithItsEvents = new List<TrackerWithItsEvents>
            {
                new TrackerWithItsEvents(tracker, allEventsTracker)
            };
            //act
            var fact = new MostEventfulDayCalculator().Calculate(trackerWithItsEvents, _now)
                .ConvertTo<MostEventfulDayTrackersFact>();

            //assert 
            Assert.True(fact.IsNone);
        }


        [Test]
        public void TrackerHaveOneEvent_CalculateFailure()
        {
            //arrange
            var userId = Guid.NewGuid();
            var tracker = CreateTrackerWithDefaultCustomization(userId);
            var events = CreateEventsEveryDayByDayInPast(tracker.Id, userId, 1, _now);
            _eventRepository.AddRangeOfEvents(events);
            var allEventsTracker = _eventRepository.LoadAllTrackerEvents(tracker.Id);
            var trackerWithItsEvents = new List<TrackerWithItsEvents>
            {
                new TrackerWithItsEvents(tracker, allEventsTracker)
            };
            //act
            var fact = new MostEventfulDayCalculator().Calculate(trackerWithItsEvents, _now)
                .ConvertTo<MostEventfulDayTrackersFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        [Test]
        public void MostEventfulAreTwoDaysWithSameEventfulnessChoseMoreLateDay_CalculateSuccess()
        {
            //arrange
            const int expectedDaysAgo1 = -15;
            const int expectedDaysAgo2 = -30;
            const int expectedSameDaysCount = 70;
            var now = DateTimeOffset.UtcNow;
            var expectedDate = now.AddDays(-expectedDaysAgo1);
            var userId = Guid.NewGuid();
            var tracker1 = CreateTrackerWithDefaultCustomization(userId);
            var tracker2 = CreateTrackerWithDefaultCustomization(userId);
            var eventsTracker1 = CreateEventsEveryDayByDayInPast(tracker1.Id, userId, 10, now);
            var eventsTracker2 = CreateEventsEveryDayByDayInPast(tracker1.Id, userId, 10, now);
            var eventsExtraEvents1 =
                CreateEventsEveryDaysAgo(tracker1.Id, userId, expectedDaysAgo1, expectedSameDaysCount, now);
            var eventsExtraEvents2 = CreateEventsEveryDaysAgo(tracker1.Id, userId, 9, 45, now);
            var eventsExtraEvents3 = CreateEventsEveryDaysAgo(tracker2.Id, userId, 5, 20, now);
            var eventsExtraEvents4 = CreateEventsEveryDaysAgo(tracker2.Id, userId, 40, 15, now);
            var eventsExtraEvents5 =
                CreateEventsEveryDaysAgo(tracker2.Id, userId, expectedDaysAgo2, expectedSameDaysCount, now);
            _eventRepository.AddRangeOfEvents(eventsTracker1);
            _eventRepository.AddRangeOfEvents(eventsTracker2);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents1);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents2);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents3);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents4);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents5);
            var allEventsTracker1 = _eventRepository.LoadAllTrackerEvents(tracker1.Id);
            var allEventsTracker2 = _eventRepository.LoadAllTrackerEvents(tracker2.Id);
            var trackerWithItsEvents = new List<TrackerWithItsEvents>
            {
                new TrackerWithItsEvents(tracker1, allEventsTracker1),
                new TrackerWithItsEvents(tracker1, allEventsTracker2)
            };

            //act
            var fact = new MostEventfulDayCalculator()
                .Calculate(trackerWithItsEvents, _now)
                .ConvertTo<MostEventfulDayTrackersFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual("Самый насыщенный событиями день", fact.FactName);
            Assert.AreEqual($"Самый насыщенный событиями день был {expectedDate:d}. Тогда произошло 70 событий",
                fact.Description);
            Assert.AreEqual(105, fact.Priority);
            Assert.AreEqual(expectedDate, fact.DayWithLargestEventsCount);
            Assert.AreEqual(expectedSameDaysCount, fact.EventsCount);
        }
    }
}