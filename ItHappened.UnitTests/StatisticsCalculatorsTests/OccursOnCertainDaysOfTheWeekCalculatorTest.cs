using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.StatisticsCalculatorsTestingConsts;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class OccursOnCertainDaysOfTheWeekCalculatorTest
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
            var eventTracker = CreateTracker();
            var events = CreateEvents_10Events_7onMonday_3onWednesday_1onTuesday(eventTracker.Id);
            _eventRepository.AddRangeOfEvents(events);

            //act 
            var fact = new OccursOnCertainDaysOfTheWeekCalculator(_eventRepository)
                .Calculate(eventTracker).ConvertTo<OccursOnCertainDaysOfTheWeekTrackerFact>().ValueUnsafe();

            //assert 
            Assert.AreEqual("Происходит в определённые дни недели", fact.FactName);
            Assert.AreEqual("В 90% случаев событие TrackerName происходит в понедельник, в среду", fact.Description);
            Assert.AreEqual(12.6, fact.Priority, PriorityAccuracy);
            Assert.AreEqual(new[] {DayOfWeek.Monday, DayOfWeek.Wednesday}, fact.DaysOfTheWeek);
            Assert.AreEqual(90.0, fact.Percentage, Percentage);
        }

        [Test]
        public void AllEventNotPasses_25PercentHitToWeekOfDayThreshold_CalculateFailure()
        {
            //arrange 
            var eventTracker = CreateTracker();
            var eventList = CreateOneEventOnEveryDay(eventTracker.Id);
            _eventRepository.AddRangeOfEvents(eventList);

            //act 
            var fact = new OccursOnCertainDaysOfTheWeekCalculator(_eventRepository)
                .Calculate(eventTracker).ConvertTo<OccursOnCertainDaysOfTheWeekTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }


        [Test]
        public void NotEnoughEvents_CalculateFailure()
        {
            //arrange 
            var eventTracker = CreateTracker();
            const int notEnoughEvents = 5;
            var eventList = CreateEvents(eventTracker.Id, notEnoughEvents);
            _eventRepository.AddRangeOfEvents(eventList);

            //act 
            var fact = new OccursOnCertainDaysOfTheWeekCalculator(_eventRepository)
                .Calculate(eventTracker).ConvertTo<OccursOnCertainDaysOfTheWeekTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        private static EventTracker CreateTracker()
        {
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            return eventTracker;
        }

        private static IEnumerable<Event> CreateEvents_10Events_7onMonday_3onWednesday_1onTuesday(Guid trackerId)
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
            return dateList.Select(date => EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, date, $"Event {date.Date.DayOfWeek}")
                    .Build())
                .ToList();
        }

        private static IEnumerable<Event> CreateOneEventOnEveryDay(Guid trackerId)
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
                .Select((t, i) =>
                    EventBuilder.Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.Now, $"Event_{i}")
                        .Build())
                .ToList();
        }
        
        
        private static IEnumerable<Event> CreateEvents(Guid trackerId, int num)
        {
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                events.Add(EventBuilder
                    .Event(Guid.NewGuid(), Guid.NewGuid(), trackerId, DateTimeOffset.UtcNow, $"Event {num}")
                    .Build());
            }
            return events;
        }
    }
}