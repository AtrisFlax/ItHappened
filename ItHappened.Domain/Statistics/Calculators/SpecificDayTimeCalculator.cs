using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class SpecificDayTimeCalculator : ISingleTrackerStatisticsCalculator
    {
        private const int EventsThreshold = 7;
        private const double CoefficientEventsPass = 0.70;
        private const double PercentageCoefficient = 0.14;

        public Option<ISingleTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker, DateTimeOffset now)
        {
            if (!CanCalculate(events))
            {
                return Option<ISingleTrackerFact>.None;
            }

            var totalEvents = events.Count;
            var daysOfTheWeek = events.GroupBy(@event => @event.HappensDate.Hour.TimeOfTheDay(),
                    (key, group) => new
                    {
                        TimeOfTheDay = key,
                        HitOnDayOfWeek = group.Count()
                    })
                .Where(groupDays => groupDays.HitOnDayOfWeek > CoefficientEventsPass * totalEvents).ToList();
            var specificDayTimeInfo = daysOfTheWeek.FirstOrDefault();
            if (specificDayTimeInfo == null)
            {
                return Option<ISingleTrackerFact>.None;
            }

            var amountEventsMoreThenPassPercent = specificDayTimeInfo.HitOnDayOfWeek;
            var percentage = 100.0d * amountEventsMoreThenPassPercent / totalEvents;
            var factName = "Происходит в определённое время суток";
            var description =
                $"В {percentage}% случаев событие {tracker.Name} происходит {specificDayTimeInfo.TimeOfTheDay}";
            var priority = PercentageCoefficient * percentage;
            var timeOfTheDay = specificDayTimeInfo.TimeOfTheDay;

            return Option<ISingleTrackerFact>.Some(new SpecificDayTimeFact(
                factName,
                description,
                priority,
                percentage,
                timeOfTheDay)
            );
        }
        
        private static bool CanCalculate(IReadOnlyCollection<Event> events)
        {
            return events.Count > EventsThreshold;
        }
    }

    internal static class DateTimeProvider
    {
        public static string TimeOfTheDay(this int hour)
        {
            return hour > 18 ? "вечером" :
                hour > 12 ? "денем" :
                hour > 6 ? "утром" : "ночью";
        }
    }
}