using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class SpecificDayTimeEventCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerStatisticsFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker))
                return Option<ISingleTrackerStatisticsFact>.None;
            
            var eventsByTimeOfTheDayWithPercent = eventTracker
                .Events
                .GroupBy(e => new {TimeOfTheDay = e.HappensDate.Hour.TimeOfTheDay(), e.Title})
                .Select(e => 
                    (
                        e.Key.Title, 
                        e.Key.TimeOfTheDay, 
                        Percentage : 100.0 * e.Count() / eventTracker.Events.Count(p => p.Title == e.Key.Title)
                    )
                );

            var (title, timeOfTheDay, percentage) = eventsByTimeOfTheDayWithPercent
                .OrderByDescending(e => e.Percentage)
                .First();
            
            return Option<ISingleTrackerStatisticsFact>.Some(new SpecificTimeOfDayEventFact
                (percentage, 
                title, 
                timeOfTheDay, eventsByTimeOfTheDayWithPercent));
        }

        private bool CanCalculate(EventTracker eventTracker) =>
            eventTracker.Events.Count > 7
            && eventTracker
                .Events
                .GroupBy(e => e.HappensDate.Hour.TimeOfTheDay())
                .Any(e => e.Count() > 0.7 * eventTracker.Events.Count);
        
    }
    
    public static class DateTimeProvider
    {
        public static string TimeOfTheDay(this int hour) => hour > 18 ? "evening" :
            hour > 12 ? "day" :
            hour > 6 ? "morning" : "night";
    }
}

