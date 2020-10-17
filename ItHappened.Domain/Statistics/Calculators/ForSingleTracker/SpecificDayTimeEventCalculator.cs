using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain.Statistics.Calculators.ForSingleTracker;
using ItHappened.Domain.Statistics.Facts.ForSingleTracker;
using LanguageExt;

namespace ItHappened.Domain.Statistics.Calculators.ForSingleTracker
{
    public class SpecificDayTimeEventCalculator : ISingleTrackerStatisticsCalculator<SpecificTimeOfDayEventFact>
    {
        public Option<SpecificTimeOfDayEventFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker))
                return Option<SpecificTimeOfDayEventFact>.None;
            
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
            
            return Option<SpecificTimeOfDayEventFact>.Some(new SpecificTimeOfDayEventFact
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

