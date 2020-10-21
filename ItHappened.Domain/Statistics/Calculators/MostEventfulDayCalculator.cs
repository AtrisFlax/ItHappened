using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class MostEventfulDayCalculator : IGeneralCalculator
    {
        private readonly IEventRepository _eventRepository;
        private const int ThresholdEventAmount = 1;

        public MostEventfulDayCalculator(IEventRepository eventRepository)
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
            var dayWithLargestEvent = trackers
                .SelectMany(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id))
                .GroupBy(@event => @event.HappensDate,
                    (date, g) => new
                    {
                        Date = date,
                        Count = g.Count()
                    }).OrderByDescending(g => g.Count).First();
            var dayWithLargestEventCount = dayWithLargestEvent.Date;
            var eventsCount = dayWithLargestEvent.Count;
            var ruEventName = RuEventName(eventsCount, "событие", "события", "событий");
            return Option<IGeneralFact>.Some(new MostEventfulDayFact(
                "Самый насыщенный событиями день",
                $"Самый насыщенный событиями день был {dayWithLargestEventCount:d}. Тогда произошло {eventsCount} {ruEventName}",
                1.5 * eventsCount,
                dayWithLargestEventCount,
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