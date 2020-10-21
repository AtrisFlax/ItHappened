using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class SpecificDayTimeEventCalculator : ISpecificCalculator
    {
        private readonly IEventRepository _eventRepository;
        
        public SpecificDayTimeEventCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        
        public Option<ISpecificFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker))
                return Option<ISpecificFact>.None;
            var eventsByTimeOfTheDayWithPercent = _eventRepository.LoadAllTrackerEvents(eventTracker.Id)
                .GroupBy(e => new {TimeOfTheDay = e.HappensDate.Hour.TimeOfTheDay(), e.Title})
                .Select(e =>
                    (
                        e.Key.Title,
                        e.Key.TimeOfTheDay,
                        Percentage: 100.0 * e.Count() / _eventRepository.LoadAllTrackerEvents(eventTracker.Id).Count(p => p.Title == e.Key.Title)
                    )
                );
            var byTimeOfTheDayWithPercent = eventsByTimeOfTheDayWithPercent.ToList();
            var (title, timeOfTheDay, percentage) = byTimeOfTheDayWithPercent
                .OrderByDescending(e => e.Percentage)
                .First();

            return Option<ISpecificFact>.Some(new SpecificTimeOfDayEventFact
            (percentage,
                title,
                timeOfTheDay, byTimeOfTheDayWithPercent));
        }

        private bool CanCalculate(EventTracker eventTracker)
        {
            var trackerEvents = _eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            return trackerEvents.Count > 7
                   && trackerEvents
                       .GroupBy(e => e.HappensDate.Hour.TimeOfTheDay())
                       .Any(e => e.Count() > 0.7 * trackerEvents.Count);
        }
    }

    public static class DateTimeProvider
    {
        public static string TimeOfTheDay(this int hour)
        {
            return hour > 18 ? "evening" :
                hour > 12 ? "day" :
                hour > 6 ? "morning" : "night";
        }
    }
}