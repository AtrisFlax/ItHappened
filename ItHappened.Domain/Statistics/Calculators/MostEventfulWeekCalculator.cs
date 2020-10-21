using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class MostEventfulWeekCalculator : IGeneralCalculator
    {
        private readonly IEventRepository _eventRepository;
        private const int ThresholdEventAmount = 1;
        private const string CultureCode = "ru-RU"; //hardcoded culture code 

        public MostEventfulWeekCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Option<IGeneralFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            var trackers = eventTrackers.ToList();
            if (!CanCalculate(trackers))
            {
                return Option<IGeneralFact>.None;
            }
            var eventfulWeek = trackers
                .SelectMany(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id))
                .GroupBy(@event => new CultureInfo(CultureCode).Calendar.GetWeekOfYear(@event.HappensDate.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday), 
                    (date, g) =>
                    {
                        var events = g.ToList();
                        return new
                        {
                            Date = date,
                            events.Count,
                            events.Min(x => x.HappensDate).Year
                        };
                    }).OrderByDescending(g => g.Count).First();
            var eventsCount = eventfulWeek.Count;
            var ruEventName = RuEventName(eventsCount, "событие", "события", "событий");
            var firstDayOfWeek =   FirstDateOfWeek(eventfulWeek.Year, eventfulWeek.Date);
            var lastDayOfWeek = firstDayOfWeek.AddDays(6);
            return Option<IGeneralFact>.Some(new MostEventfulWeekFact(
                "Самая насыщенная событиями неделя",
                $"Самая насыщенная событиями неделя была с {firstDayOfWeek:d} до {lastDayOfWeek:d}. За её время произошло {eventsCount} {ruEventName}",
                0.75 * eventsCount,
                firstDayOfWeek,
                lastDayOfWeek,
                eventsCount
            ));
        }

        private static string RuEventName(int number, string nominativ, string genetiv, string plural)
        {
            var titles = new[] {nominativ, genetiv, plural};
            var cases = new[] {2, 0, 1, 1, 1, 2};
            return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[number % 10 < 5 ? number % 10 : 5]];
        }

        private bool CanCalculate(IEnumerable<EventTracker> eventTracker)
        {
            return eventTracker.Any(tracker =>
                _eventRepository.LoadAllTrackerEvents(tracker.Id).Count > ThresholdEventAmount);
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
    }
}