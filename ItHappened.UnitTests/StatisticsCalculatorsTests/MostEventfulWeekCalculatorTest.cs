// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.Linq;
// using ItHappened.Domain;
// using ItHappened.Domain.Statistics;
// using ItHappened.Infrastructure.Repositories;
// using LanguageExt.UnsafeValueAccess;
// using NUnit.Framework;
//
// namespace ItHappened.UnitTests.StatisticsCalculatorsTests
// {
//     public class MostEventfulWeekCalculatorTest
//     {
//         private IEventRepository _eventRepository;
//         private static Random _rand;
//         private const string CultureCode = "ru-RU"; //hardcoded culture code 
//
//         [SetUp]
//         public void Init()
//         {
//             _eventRepository = new EventRepository();
//             _rand = new Random();
//         }
//         
//         [Repeat( 1000 )]
//         [Test]
//         public void FindInTwoTrackerWithFactMostEventfulWeek_CalculateSuccess()
//         {
//             //arrange 
//             var userId = Guid.NewGuid();
//             var eventTracker1 = CreateTrackerWithDefaultCustomization(userId, "Покупка");
//             var eventTracker2 = CreateTrackerWithDefaultCustomization(userId, "Продажа");
//             const int year = 2020;
//             var eventsOfTracker1 = CreateEventDuringYear(eventTracker1.Id, userId, 1000, year);
//             var eventsOfTracker2 = CreateEventDuringYear(eventTracker2.Id, userId, 1000, year);
//             _eventRepository.AddRangeOfEvents(eventsOfTracker1);
//             _eventRepository.AddRangeOfEvents(eventsOfTracker2);
//             var allEvents = new List<Event>();
//             allEvents.AddRange(eventsOfTracker1);
//             allEvents.AddRange(eventsOfTracker2);
//             var eventfulWeek = allEvents
//                 .GroupBy(GetWeekNum,
//                     (weekNum, g) => new
//                     {
//                         WeekNum = weekNum,
//                         Count = g.Count()
//                     })
//                 .OrderByDescending(g => g.Count).First();
//             var firstDayOfWeek = FirstDateOfWeek(year, eventfulWeek.WeekNum);
//             var lastDayOfWeek = firstDayOfWeek.AddDays(6);
//
//             //act
//             var fact = new MostEventfulWeekCalculator(_eventRepository)
//                 .Calculate(new[] {eventTracker1, eventTracker2})
//                 .ConvertTo<MostEventfulWeekTrackersFact>().ValueUnsafe();
//
//             //assert 
//             var ruName = RuEventName(eventfulWeek.Count, "событие", "события", "событий");
//             Assert.AreEqual("Самая насыщенная событиями неделя", fact.FactName);
//             Assert.AreEqual(
//                 $@"Самая насыщенная событиями неделя была с {firstDayOfWeek:d} до {lastDayOfWeek:d}. За её время произошло {eventfulWeek.Count} {ruName}",
//                 fact.Description);
//             Assert.AreEqual(0.75*eventfulWeek.Count, fact.Priority);
//             Assert.AreEqual(firstDayOfWeek, fact.WeekWithLargestEventCountFirstDay);
//             Assert.AreEqual(lastDayOfWeek, fact.WeekWithLargestEventCountLastDay);
//             Assert.AreEqual(eventfulWeek.Count, fact.EventsCount);
//         }
//         
//         [Test]
//         public void TrackerHaveZeroEvent_CalculateFailure()
//         {
//             //arrange
//             var now = DateTimeOffset.UtcNow;
//             var userId = Guid.NewGuid();
//             const int year = 2020;
//             var eventTracker = CreateTrackerWithDefaultCustomization(userId, "Покупка");
//             var eventsTracker = CreateEventDuringYear(eventTracker.Id, userId, 0, year);
//             _eventRepository.AddRangeOfEvents(eventsTracker);
//
//             //act
//             var fact = new MostEventfulDayCalculator(_eventRepository)
//                 .Calculate(new[] {eventTracker})
//                 .ConvertTo<MostEventfulDayTrackersFact>();
//
//             //assert 
//             Assert.True(fact.IsNone);
//         }
//         
//         
//         [Test]
//         public void TrackerHaveOneEvent_CalculateFailure()
//         {
//             //arrange
//             var now = DateTimeOffset.UtcNow;
//             var userId = Guid.NewGuid();
//             const int year = 2020;
//             var eventTracker = CreateTrackerWithDefaultCustomization(userId, "Покупка");
//             var eventsTracker = CreateEventDuringYear(eventTracker.Id, userId, 1, year);
//             _eventRepository.AddRangeOfEvents(eventsTracker);
//
//             //act
//             var fact = new MostEventfulDayCalculator(_eventRepository)
//                 .Calculate(new[] {eventTracker})
//                 .ConvertTo<MostEventfulDayTrackersFact>();
//
//             //assert 
//             Assert.True(fact.IsNone);
//         }
//
//         private static int GetWeekNum(Event @event)
//         {
//             return new CultureInfo(CultureCode).Calendar.GetWeekOfYear(@event.HappensDate.Date,
//                 CalendarWeekRule.FirstDay, DayOfWeek.Monday);
//         }
//
//         private static EventTracker CreateTrackerWithDefaultCustomization(Guid userId, string trackerName)
//         {
//             return EventTrackerBuilder
//                 .Tracker(userId, Guid.NewGuid(), trackerName)
//                 .Build();
//         }
//
//         private static IList<Event> CreateEventDuringYear(Guid eventTrackerId, Guid userId, int num, int year)
//         {
//             var events = new List<Event>();
//             for (var i = 0; i < num; i++)
//             {
//                 var newEvent = CreateEvent(userId, eventTrackerId, "For Tracker 1", RandomDayDuringYear(year));
//                 events.Add(newEvent);
//             }
//
//             return events;
//         }
//
//         private static Event CreateEvent(Guid userId, Guid trackerId, string title,
//             DateTimeOffset dateTime)
//         {
//             return EventBuilder.Event(Guid.NewGuid(), userId, trackerId, dateTime, title).Build();
//         }
//
//         private static DateTimeOffset RandomDayDuringYear(int year)
//         {
//             var startYear = new DateTime(year, 1, 1);
//             var start = new DateTimeOffset(startYear);
//             var end = new DateTimeOffset(startYear.AddYears(1).AddDays(-1));
//             var range = (end - start).Days;
//             return start.AddDays(_rand.Next(range));
//         }
//
//         private static DateTimeOffset FirstDateOfWeek(int year, int weekOfYear)
//         {
//             var jan1 = new DateTime(year, 1, 1);
//             var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
//             var firstThursday = jan1.AddDays(daysOffset);
//             var cal = new CultureInfo(CultureCode).Calendar;
//             var firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
//             var weekNum = weekOfYear;
//             if (firstWeek == 1)
//             {
//                 weekNum -= 1;
//             }
//
//             var result = firstThursday.AddDays(weekNum * 7);
//             return result.AddDays(-3);
//         }
//
//         private static string RuEventName(int number, string nominativ, string genetiv, string plural)
//         {
//             var titles = new[] {nominativ, genetiv, plural};
//             var cases = new[] {2, 0, 1, 1, 1, 2};
//             return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[number % 10 < 5 ? number % 10 : 5]];
//         }
//     }
// }