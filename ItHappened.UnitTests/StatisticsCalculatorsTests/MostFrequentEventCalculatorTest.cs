using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConsts;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class MostFrequentEventCalculatorTest
    {
        private IEventRepository _eventRepository;

        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
        }

        [Test]
        public void CreateTwoEventTrackersWithEnoughEvents_GetMostFrequentEventFact_CheckAllProperties()
        {
            //arrange
            var userId = Guid.NewGuid();
            const string expectedTrackerName = "Pains after drinking sugar water";
            var tracker1 = CreateTracker(userId, name: expectedTrackerName);
            var tracker2 = CreateTracker(userId, "Pains after eating sugar");
            var events =
                CreateEventsEnoughEventForCalculateForTwoTrackers(tracker1.Id, tracker2.Id, Guid.NewGuid());
            _eventRepository.AddRangeOfEvents(events.tracker1);
            _eventRepository.AddRangeOfEvents(events.tracker2);
            var trackerWithItsEvents = new List<TrackerWithItsEvents>
            {
                new TrackerWithItsEvents(tracker1, events.tracker1),
                new TrackerWithItsEvents(tracker2, events.tracker2)
            };

            //act
            var fact = new MostFrequentEventStatisticsCalculator()
                .Calculate(trackerWithItsEvents)
                .ConvertTo<MostFrequentEventTrackerTrackerFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual("Самое частое событие", fact.FactName);
            Assert.AreEqual(
                $"Чаще всего у вас происходит событие {expectedTrackerName} - раз в {fact.EventsPeriod:0.#} дней",
                fact.Description);
            Assert.AreEqual(4.0, fact.Priority, PriorityAccuracy);
            Assert.AreEqual(expectedTrackerName, fact.TrackingName);
            Assert.AreEqual(2.5, fact.EventsPeriod, EventsPeriodAccuracy);
        }

        //TODO test with only one tracker (==1)

        //TODO test enough tracker but not enough events in one tracker (<=3)

        //TODO test enough tracker but not enough events in all trackers (<=3)


        private static (List<Event> tracker1, List<Event> tracker2) CreateEventsEnoughEventForCalculateForTwoTrackers(
            Guid eventTracker1Id, Guid eventTracker2Id,
            Guid userId)
        {
            var now = DateTimeOffset.Now;
            var tracker1Events = new List<Event>
            {
                CreateEventFixDate(userId, eventTracker1Id, now.AddDays(-1)),
                CreateEventFixDate(userId, eventTracker1Id, now.AddDays(-2)),
                CreateEventFixDate(userId, eventTracker1Id, now.AddDays(-3)),
                CreateEventFixDate(userId, eventTracker1Id, now.AddDays(-10)) //first by time event in tracker1
            };
            var tracker2Events = new List<Event>
            {
                CreateEventFixDate(userId, eventTracker2Id, now.AddDays(-1)),
                CreateEventFixDate(userId, eventTracker2Id, now.AddDays(-2)),
                CreateEventFixDate(userId, eventTracker2Id, now.AddDays(-3)),
                CreateEventFixDate(userId, eventTracker2Id, now.AddDays(-4)),
                CreateEventFixDate(userId, eventTracker2Id, now.AddDays(-15)) //first by time event in tracker2
            };
            return (tracker1Events, tracker2Events);
        }
    }
}