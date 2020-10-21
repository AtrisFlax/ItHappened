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
            var weekWithBiggestEventCount = trackers
                .SelectMany(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id))
                .GroupBy(@event => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(@event.HappensDate.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday), 
                    (date, g) => new
                    {
                        Date = date,
                        Count = g.Count()
                    }).OrderByDescending(g => g.Count).First();
            var eventsCount = weekWithBiggestEventCount.Count;
            var ruEventName = RuEventName(eventsCount, "событие", "события", "событий");
            var weekWithLargestEventCountFirstDay =  DateTimeOffset.Now;
            var weekWithLargestEventCountLastDay = DateTimeOffset.Now;
            return Option<IGeneralFact>.Some(new MostEventfulWeekFact(
                "Самая насыщенная событиями неделя",
                $"Самая насыщенная событиями неделя была с {weekWithLargestEventCountFirstDay:d} до {weekWithLargestEventCountLastDay:d}. За её время произошло {eventsCount} {ruEventName}",
                0.75 * eventsCount,
                weekWithLargestEventCountFirstDay,
                weekWithLargestEventCountLastDay,
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
    }
}