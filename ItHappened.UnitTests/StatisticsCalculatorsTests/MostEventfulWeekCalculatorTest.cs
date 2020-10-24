﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Repositories;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using static ItHappened.UnitTests.StatisticsCalculatorsTests.TestingMethods;
namespace ItHappened.UnitTests.StatisticsCalculatorsTests
{
    public class MostEventfulWeekCalculatorTest
    {
        private IEventRepository _eventRepository;
        private static Random _rand;
        private const string CultureCode = "ru-RU"; //hardcoded culture code 

        [SetUp]
        public void Init()
        {
            _eventRepository = new EventRepository();
            _rand = new Random();
        }
        
        [Repeat( 1000 )]
        [Test]
        public void FindInTwoTrackerWithFactMostEventfulWeek_CalculateSuccess()
        {
            //arrange 
            var userId = Guid.NewGuid();
            var tracker1 = CreateTracker(userId);
            var tracker2 = CreateTracker(userId);
            const int year = 2020;
            var eventsOfTracker1 = CreateEventDuringYear(tracker1.Id, userId, 1000, year);
            var eventsOfTracker2 = CreateEventDuringYear(tracker2.Id, userId, 1000, year);
            _eventRepository.AddRangeOfEvents(eventsOfTracker1);
            _eventRepository.AddRangeOfEvents(eventsOfTracker2);
            var allEvents = new List<Event>();
            allEvents.AddRange(eventsOfTracker1);
            allEvents.AddRange(eventsOfTracker2);
            var eventfulWeek = allEvents
                .GroupBy(GetWeekNum,
                    (weekNum, g) => new
                    {
                        WeekNum = weekNum,
                        Count = g.Count()
                    })
                .OrderByDescending(g => g.Count).First();
            var firstDayOfWeek = FirstDateOfWeek(year, eventfulWeek.WeekNum);
            var lastDayOfWeek = firstDayOfWeek.AddDays(6);
            var allEventsTracker1 = _eventRepository.LoadAllTrackerEvents(tracker1.Id);
            var allEventsTracker2 = _eventRepository.LoadAllTrackerEvents(tracker2.Id);
            var trackerWithItsEvents = new List<TrackerWithItsEvents>
            {
                new TrackerWithItsEvents(tracker1, allEventsTracker1),
                new TrackerWithItsEvents(tracker2, allEventsTracker2)
            };
            
            //act
            var fact = new MostEventfulWeekCalculator()
                .Calculate(trackerWithItsEvents)
                .ConvertTo<MostEventfulWeekTrackerTrackerFact>().ValueUnsafe();

            //assert 
            var ruName = RuEventName(eventfulWeek.Count, "событие", "события", "событий");
            Assert.AreEqual("Самая насыщенная событиями неделя", fact.FactName);
            Assert.AreEqual(
                $@"Самая насыщенная событиями неделя была с {firstDayOfWeek:d} до {lastDayOfWeek:d}. За её время произошло {eventfulWeek.Count} {ruName}",
                fact.Description);
            Assert.AreEqual(0.75*eventfulWeek.Count, fact.Priority);
            Assert.AreEqual(firstDayOfWeek, fact.WeekWithLargestEventCountFirstDay);
            Assert.AreEqual(lastDayOfWeek, fact.WeekWithLargestEventCountLastDay);
            Assert.AreEqual(eventfulWeek.Count, fact.EventsCount);
        }
        
        [Test]
        public void TrackerHaveZeroEvent_CalculateFailure()
        {
            //arrange
            var userId = Guid.NewGuid();
            const int year = 2020;
            var tracker = CreateTracker(userId);
            var trackerEvents = CreateEventDuringYear(tracker.Id, userId, 0, year);
            _eventRepository.AddRangeOfEvents(trackerEvents);
            var allEventsTracker1 = _eventRepository.LoadAllTrackerEvents(tracker.Id);
            var trackerWithItsEvents = new List<TrackerWithItsEvents>
            {
                new TrackerWithItsEvents(tracker, allEventsTracker1)
            };
            
            //act
            var fact = new MostEventfulWeekCalculator()
                .Calculate(trackerWithItsEvents)
                .ConvertTo<MostEventfulDayTrackerTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }
        
        
        [Test]
        public void TrackerHaveOneEvent_CalculateFailure()
        {
            //arrange
            var userId = Guid.NewGuid();
            const int year = 2020;
            var tracker = CreateTracker(userId);
            var trackerEvents = CreateEventDuringYear(tracker.Id, userId, 1, year);
            _eventRepository.AddRangeOfEvents(trackerEvents);
            var allEventsTracker1 = _eventRepository.LoadAllTrackerEvents(tracker.Id);
            var trackerWithItsEvents = new List<TrackerWithItsEvents>
            {
                new TrackerWithItsEvents(tracker, allEventsTracker1)
            };
            
            //act
            var fact = new MostEventfulWeekCalculator()
                .Calculate(trackerWithItsEvents)
                .ConvertTo<MostEventfulDayTrackerTrackerFact>();

            //assert 
            Assert.True(fact.IsNone);
        }

        private static int GetWeekNum(Event @event)
        {
            return new CultureInfo(CultureCode).Calendar.GetWeekOfYear(@event.HappensDate.Date,
                CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
        
        private static IList<Event> CreateEventDuringYear(Guid eventTrackerId, Guid userId, int num, int year)
        {
            var events = new List<Event>();
            for (var i = 0; i < num; i++)
            {
                var newEvent = CreateEventFixDate(eventTrackerId, userId, RandomDayDuringYear(year));
                events.Add(newEvent);
            }

            return events;
        }
        
        private static DateTimeOffset RandomDayDuringYear(int year)
        {
            var startYear = new DateTime(year, 1, 1);
            var start = new DateTimeOffset(startYear);
            var end = new DateTimeOffset(startYear.AddYears(1).AddDays(-1));
            var range = (end - start).Days;
            return start.AddDays(_rand.Next(range));
        }

        private static DateTimeOffset FirstDateOfWeek(int year, int weekOfYear)
        {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
            var firstThursday = jan1.AddDays(daysOffset);
            var cal = new CultureInfo(CultureCode).Calendar;
            var firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var weekNum = weekOfYear;
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        private static string RuEventName(int number, string nominativ, string genetiv, string plural)
        {
            var titles = new[] {nominativ, genetiv, plural};
            var cases = new[] {2, 0, 1, 1, 1, 2};
            return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[number % 10 < 5 ? number % 10 : 5]];
        }
    }
}