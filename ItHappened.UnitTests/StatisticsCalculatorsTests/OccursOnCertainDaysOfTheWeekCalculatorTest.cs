using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain.Statistics;
using ItHappened.Domain;
using NUnit.Framework;

namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class OccursOnCertainDaysOfTheWeekCalculatorTest
    {
        [Test]
        public void EventTrackerHasTwoRatingAndEvents_CalculateSuccess()
        {
            //arrange 
            var eventList = CreateEvents_10Events_7OnMonday_3OnWednesday_1Tuesday();
            var eventTracker = CreateTracker(eventList);

            //act 
            var fact = new OccursOnCertainDaysOfTheWeekCalculator().Calculate(eventTracker).ConvertTo<OccursOnCertainDaysOfTheWeekFact>();
            //assert 
            Assert.True(fact.IsSome);

            fact.Do(f =>
            {
                Assert.AreEqual("Происходит в определённые дни недели", f.FactName);
                Assert.AreEqual("В 90% случаев событие TrackerName происходит в понедельник, в среду", f.Description);
                Assert.AreEqual(12.6, f.Priority, 1e-5);
                Assert.AreEqual(new[] {DayOfWeek.Monday, DayOfWeek.Wednesday}, f.DaysOfTheWeek);
                Assert.AreEqual(90.0, f.Percentage, 1e-5);
            });
        }
        
        [Test]
        public void AllEventNotPasses_25PercentHitToWeekOfDayThreshold_CalculateFailure()
        {
            //arrange 
            var eventList = CreateEvents_10Events_7OnMonday_3OnWednesday_1Tuesday();
            var eventTracker = CreateTracker(eventList);

            //act 
            var fact = new OccursOnCertainDaysOfTheWeekCalculator().Calculate(eventTracker).ConvertTo<OccursOnCertainDaysOfTheWeekFact>();

            //assert 
            Assert.True(fact.IsSome);

            fact.Do(f =>
            {
                Assert.AreEqual("Происходит в определённые дни недели", f.FactName);
                Assert.AreEqual("В 90% случаев событие TrackerName происходит в понедельник, в среду", f.Description);
                Assert.AreEqual(12.6, f.Priority, 1e-5);
                Assert.AreEqual(new[] {DayOfWeek.Monday, DayOfWeek.Wednesday}, f.DaysOfTheWeek);
                Assert.AreEqual(90.0, f.Percentage, 1e-5);
            });
        }


        [Test]
        public void NotEnoughEvents_CalculateFailure()
        {
            //arrange 
            var eventList = CreateOneEventOnEveryDay();
            var eventTracker = CreateTracker(eventList);

            //act 
            var fact = new OccursOnCertainDaysOfTheWeekCalculator().Calculate(eventTracker).ConvertTo<OccursOnCertainDaysOfTheWeekFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        private static EventTracker CreateTracker(List<Event> eventList)
        {
            var eventTracker = EventTrackerBuilder
                .Tracker(Guid.NewGuid(), Guid.NewGuid(), "TrackerName")
                .Build();
            AddEvents(eventList, eventTracker);
            return eventTracker;
        }

        private static void AddEvents(List<Event> eventList, EventTracker eventTracker)
        {
            foreach (var @event in eventList)
            {
                eventTracker.AddEvent(@event);
            }
        }

        private static List<Event> CreateEvents_10Events_7OnMonday_3OnWednesday_1Tuesday()
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
                .Select((t, i) =>
                    EventBuilder.Event(Guid.NewGuid(), Guid.NewGuid(), t, $"Event_{i}").Build())
                .ToList();
        }
        
        private static List<Event> CreateOneEventOnEveryDay()
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
                    EventBuilder.Event(Guid.NewGuid(), Guid.NewGuid(), t, $"Event_{i}").Build())
                .ToList();
        }
    }
}

