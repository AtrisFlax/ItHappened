using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure;

using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConstants;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;


namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class MostFrequentEventCalculatorTest
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
        public void CreateTwoEventTrackersWithEnoughEvents_GetMostFrequentEventFact_CheckAllProperties()
        {
            //arrange
            var now = DateTime.Now;
            var userId = Guid.NewGuid();
            var tracker1 = CreateTrackerWithDefaultCustomization(userId, "Pains after drinking sugar water");
            var tracker2 = CreateTrackerWithDefaultCustomization(userId, "Pains after eating sugar");
            var eventsOfTracker1 = EventsForFirstTracker(tracker1.Id, userId, now);
            var eventsOfTracker2 = EventsForSecondTracker(tracker2.Id, userId, now);
            _eventRepository.AddRangeOfEvents(eventsOfTracker1);
            _eventRepository.AddRangeOfEvents(eventsOfTracker2);
            var allEventsTracker1 = _eventRepository.LoadAllTrackerEvents(tracker1.Id);
            var allEventsTracker2 = _eventRepository.LoadAllTrackerEvents(tracker2.Id);
            var trackerWithItsEvents = new List<TrackerWithItsEvents>
            {
                new TrackerWithItsEvents(tracker1, allEventsTracker1),
                new TrackerWithItsEvents(tracker1, allEventsTracker2)
            };

            //act
            var fact = new MostFrequentEventStatisticsCalculator()
                .Calculate(trackerWithItsEvents, _now)
                .ConvertTo<MostFrequentEventTrackersFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual("Самое частое событие", fact.FactName);
            Assert.AreEqual(
                $"Чаще всего у вас происходит событие Pains after drinking sugar water - раз в {fact.EventsPeriod:0.#} дней",
                fact.Description);
            Assert.AreEqual(4.0, fact.Priority, PriorityAccuracy);
            Assert.AreEqual("Pains after drinking sugar water", fact.TrackingName);
            Assert.AreEqual(2.5, fact.EventsPeriod, EventsPeriodAccuracy);
        }

        //TODO test with only one tracker (==1)

        //TODO test enough tracker but not enough events in one tracker (<=3)

        //TODO test enough tracker but not enough events in all trackers (<=3)


        private static IEnumerable<Event> EventsForFirstTracker(Guid trackerId, Guid userId, DateTimeOffset now)
        {
            var event1
                =
                CreateEventWithFixTime(trackerId, userId, now.AddDays(-1));
            var event2 =
                CreateEventWithFixTime(trackerId, userId, now.AddDays(-2));
            var event3 =
                CreateEventWithFixTime(trackerId, userId, now.AddDays(-3));
            var event4 =
                CreateEventWithFixTime(trackerId, userId, now.AddDays(-10)); //first event
            return new[]
            {
                event1,
                event2,
                event3,
                event4
            };
        }

        private static IEnumerable<Event> EventsForSecondTracker(Guid trackerId, Guid userId, DateTimeOffset now)
        {
            var event1 = CreateEventWithFixTime(trackerId, userId, now.AddDays(-1));
            var event2 = CreateEventWithFixTime(trackerId, userId, now.AddDays(-2));
            var event3 = CreateEventWithFixTime(trackerId, userId, now.AddDays(-3));
            var event4 = CreateEventWithFixTime(trackerId, userId, now.AddDays(-3));
            var event5 = CreateEventWithFixTime(trackerId, userId, now.AddDays(-15)); //first event
            return new[]
            {
                event1,
                event2,
                event3,
                event4,
                event5
            };
        }
    }
}