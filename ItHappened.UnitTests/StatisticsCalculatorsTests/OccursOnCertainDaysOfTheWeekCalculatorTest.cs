using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConstants;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class OccursOnCertainDaysOfTheWeekCalculator
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
        public void EventTrackerHasTwoRatingAndEvents_CalculateSuccess()
        {
            //arrange 
            var userId = Guid.NewGuid();
            var tracker = CreateTracker(userId, "some name");
            var events = CreateEvents_10Events_7onMonday_3onWednesday_1onTuesday(tracker.Id, userId);
            _eventRepository.AddRangeOfEvents(events);

            //act 
            var fact = new Domain.Statistics.OccursOnCertainDaysOfTheWeekCalculator()
                .Calculate(events, tracker, _now).ConvertTo<OccursOnCertainDaysOfTheWeekTrackerFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual("Происходит в определённые дни недели", fact.FactName);
            Assert.AreEqual($"В 90% случаев событие {tracker.Name} происходит в понедельник, в среду",
                fact.Description);
            Assert.AreEqual(12.6, fact.Priority, PriorityAccuracy);
            Assert.AreEqual(new[] {DayOfWeek.Monday, DayOfWeek.Wednesday}, fact.DaysOfTheWeek);
            Assert.AreEqual(90.0, fact.Percentage, Percentage);
        }

        [Test]
        public void AllEventNotPasses_25PercentHitToWeekOfDayThreshold_CalculateFailure()
        {
            //arrange 
            var userId = Guid.NewGuid();
            var tracker = CreateTracker(userId);
            var events = CreateOneEventOnEveryDay(tracker.Id, userId);
            _eventRepository.AddRangeOfEvents(events);

            //act 
            var fact = new Domain.Statistics.OccursOnCertainDaysOfTheWeekCalculator()
                .Calculate(events, tracker, _now).ConvertTo<OccursOnCertainDaysOfTheWeekTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }


        [Test]
        public void NotEnoughEvents_CalculateFailure()
        {
            //arrange 
            var userId = Guid.NewGuid();
            var tracker = CreateTracker(userId);
            const int notEnoughEvents = 5;
            var events = CreateEvents(userId, tracker.Id, notEnoughEvents);
            _eventRepository.AddRangeOfEvents(events);

            //act 
            var fact = new Domain.Statistics.OccursOnCertainDaysOfTheWeekCalculator()
                .Calculate(events, tracker, _now).ConvertTo<OccursOnCertainDaysOfTheWeekTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        private static IReadOnlyCollection<Event> CreateEvents_10Events_7onMonday_3onWednesday_1onTuesday(
            Guid trackerId,
            Guid userId)
        {
            var monday = new DateTime(2020, 10, 5);
            var tuesday = new DateTime(2020, 10, 6);
            var wednesday = new DateTime(2020, 10, 7);
            var dateList = new List<DateTimeOffset>
            {
                new DateTimeOffset(monday),
                new DateTimeOffset(monday),
                new DateTimeOffset(monday),
                new DateTimeOffset(monday),
                new DateTimeOffset(monday),
                new DateTimeOffset(monday),
                new DateTimeOffset(tuesday),
                new DateTimeOffset(wednesday),
                new DateTimeOffset(wednesday),
                new DateTimeOffset(wednesday),
            };
            return dateList
                .Select((t, i) => CreateEventWithFixTime(trackerId, userId, t))
                .ToList().AsReadOnly();
        }

        private static IReadOnlyCollection<Event> CreateOneEventOnEveryDay(Guid userId, Guid trackerId)
        {
            var monday = new DateTime(2020, 10, 5);
            var dateList = new List<DateTimeOffset>
            {
                new DateTimeOffset(monday),
                new DateTimeOffset(monday.AddDays(1)),
                new DateTimeOffset(monday.AddDays(2)),
                new DateTimeOffset(monday.AddDays(3)),
                new DateTimeOffset(monday.AddDays(4)),
                new DateTimeOffset(monday.AddDays(5)),
                new DateTimeOffset(monday.AddDays(6)),
            };

            return dateList
                .Select((t, i) => CreateEventWithFixTime(trackerId, userId, t))
                .ToList().AsReadOnly();
        }
    }
}