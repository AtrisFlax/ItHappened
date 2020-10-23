using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class MostEventfulDayCalculatorTest
    {
        private IEventRepository _eventRepository;

        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
        }

        [Test]
        public void FindInTwoTrackerWithFactMostEventfulDay_CalculateSuccess()
        {
            //arrange
            const int expectedDaysAgo = -15;
            var now = DateTimeOffset.UtcNow;
            var expectedDate = now.AddDays(-expectedDaysAgo);
            var userId = Guid.NewGuid();
            var eventTracker1 = CreateTracker(userId, "Покупка");
            var eventTracker2 = CreateTracker(userId, "Продажа");
            var eventsTracker1 = CreateEventsEveryDayByDayInPast(eventTracker1.Id, userId, 10, now);
            var eventsTracker2 = CreateEventsEveryDayByDayInPast(eventTracker1.Id, userId, 10, now);
            var eventsExtraEvents1 =
                CreateEventsEveryDaysAgo(eventTracker1.Id, userId, expectedDaysAgo, 70, now);
            var eventsExtraEvents2 = CreateEventsEveryDaysAgo(eventTracker1.Id, userId, 9, 45, now);
            var eventsExtraEvents3 = CreateEventsEveryDaysAgo(eventTracker2.Id, userId, 5, 20, now);
            var eventsExtraEvents4 = CreateEventsEveryDaysAgo(eventTracker2.Id, userId, 40, 15, now);
            _eventRepository.AddRangeOfEvents(eventsTracker1);
            _eventRepository.AddRangeOfEvents(eventsTracker2);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents1);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents2);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents3);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents4);

            //act
            var fact = new MostEventfulDayStatisticsCalculator(_eventRepository)
                .Calculate(new[] {eventTracker1, eventTracker2})
                .ConvertTo<MostEventfulDayTrackerTrackerFact>().ValueUnsafe();

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
            var now = DateTimeOffset.UtcNow;
            var userId = Guid.NewGuid();
            var eventTracker = CreateTracker(userId, "Покупка");
            var eventsTracker = CreateEventsEveryDayByDayInPast(eventTracker.Id, userId, 0, now);
            _eventRepository.AddRangeOfEvents(eventsTracker);

            //act
            var fact = new MostEventfulDayStatisticsCalculator(_eventRepository)
                .Calculate(new[] {eventTracker})
                .ConvertTo<MostEventfulDayTrackerTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }
        
        
        [Test]
        public void TrackerHaveOneEvent_CalculateFailure()
        {
            //arrange
            var now = DateTimeOffset.UtcNow;
            var userId = Guid.NewGuid();
            var eventTracker = CreateTracker(userId, "Покупка");
            var eventsTracker = CreateEventsEveryDayByDayInPast(eventTracker.Id, userId, 1, now);
            _eventRepository.AddRangeOfEvents(eventsTracker);

            //act
            var fact = new MostEventfulDayStatisticsCalculator(_eventRepository)
                .Calculate(new[] {eventTracker})
                .ConvertTo<MostEventfulDayTrackerTrackerFact>();

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
            var eventTracker1 = CreateTracker(userId, "Покупка");
            var eventTracker2 = CreateTracker(userId, "Продажа");
            var eventsTracker1 = CreateEventsEveryDayByDayInPast(eventTracker1.Id, userId, 10, now);
            var eventsTracker2 = CreateEventsEveryDayByDayInPast(eventTracker1.Id, userId, 10, now);
            var eventsExtraEvents1 =
                CreateEventsEveryDaysAgo(eventTracker1.Id, userId, expectedDaysAgo1, expectedSameDaysCount, now);
            var eventsExtraEvents2 = CreateEventsEveryDaysAgo(eventTracker1.Id, userId, 9, 45, now);
            var eventsExtraEvents3 = CreateEventsEveryDaysAgo(eventTracker2.Id, userId, 5, 20, now);
            var eventsExtraEvents4 = CreateEventsEveryDaysAgo(eventTracker2.Id, userId, 40, 15, now);
            var eventsExtraEvents5 =
                CreateEventsEveryDaysAgo(eventTracker2.Id, userId, expectedDaysAgo2, expectedSameDaysCount, now);
            _eventRepository.AddRangeOfEvents(eventsTracker1);
            _eventRepository.AddRangeOfEvents(eventsTracker2);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents1);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents2);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents3);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents4);
            _eventRepository.AddRangeOfEvents(eventsExtraEvents5);

            //act
            var fact = new MostEventfulDayStatisticsCalculator(_eventRepository)
                .Calculate(new[] {eventTracker1, eventTracker2})
                .ConvertTo<MostEventfulDayTrackerTrackerFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual("Самый насыщенный событиями день", fact.FactName);
            Assert.AreEqual($"Самый насыщенный событиями день был {expectedDate:d}. Тогда произошло 70 событий",
                fact.Description);
            Assert.AreEqual(105, fact.Priority);
            Assert.AreEqual(expectedDate, fact.DayWithLargestEventsCount);
            Assert.AreEqual(expectedSameDaysCount, fact.EventsCount);
        }

        private static EventTracker CreateTracker(Guid userId, string trackerName)
        {
            var eventTracker1 = EventTrackerBuilder
                .Tracker(userId, Guid.NewGuid(), trackerName)
                .Build();
            return eventTracker1;
        }

        private static IEnumerable<Event> CreateEventsEveryDayByDayInPast(Guid eventTrackerId, Guid userId, int num,
            DateTimeOffset time)
        {
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(CreateEvent(userId, eventTrackerId, "For Tracker 1", time.AddDays(-i)));
            }

            return events;
        }

        private static Event CreateEvent(Guid userId, Guid trackerId, string title,
            DateTimeOffset dateTime)
        {
            return EventBuilder.Event(Guid.NewGuid(), userId, trackerId, dateTime, title).Build();
        }

        private static IEnumerable<Event> CreateEventsEveryDaysAgo(Guid eventTrackerId, Guid userId, int daysAgo,
            int num, DateTimeOffset now)
        {
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(CreateEvent(userId, eventTrackerId, "For Tracker 1", now.AddDays(-daysAgo)));
            }

            return events;
        }
    }
}