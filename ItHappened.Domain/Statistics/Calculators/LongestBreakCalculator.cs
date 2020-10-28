using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class LongestBreakCalculator : ISingleTrackerStatisticsCalculator
    {
        private const int EventsThreshold = 10;
        private const int DaysThreshold = 7;

        public Option<ISingleTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker, DateTimeOffset now)
        {
            if (!CanCalculate(events))
            {
                return Option<ISingleTrackerFact>.None;
            }

            var (lastEventBeforeBreak, firstEventAfterBreak) = GetFirstAndLastEventOfTheLongestBreak(events);
            var maxDurationInDays = (firstEventAfterBreak.HappensDate - lastEventBeforeBreak.HappensDate).Days;
            var priority = Math.Sqrt(maxDurationInDays);
            var description =
                $"Самый большой перерыв в {tracker.Name} произошёл с {lastEventBeforeBreak}" +
                $" до {firstEventAfterBreak}, он занял {maxDurationInDays} дней";
            const string factName = "Самый долгий перерыв";

            return Option<ISingleTrackerFact>.Some(new LongestBreakTrackerFact(
                factName,
                description,
                priority,
                maxDurationInDays,
                lastEventBeforeBreak.HappensDate,
                firstEventAfterBreak.HappensDate));
        }

        private static bool CanCalculate(IReadOnlyCollection<Event> events)
        {
            if (events.Count <= EventsThreshold)
                return false;

            var (lastEventBeforeBreak, firstEventAfterBreak) = GetFirstAndLastEventOfTheLongestBreak(events);
            var maxDuration = (firstEventAfterBreak.HappensDate - lastEventBeforeBreak.HappensDate).Days;
            if ((firstEventAfterBreak.HappensDate - DateTimeOffset.Now).Days > DaysThreshold)
                return false;

            var averageTimeBetweenEvents = GetAverageDurationBetweenEvents(events);
            return !(maxDuration < averageTimeBetweenEvents * 3);
        }

        private static (Event lastEventBeforeBreak, Event firstEventAfterBreak) GetFirstAndLastEventOfTheLongestBreak(
            IReadOnlyCollection<Event> events)
        {
            var listOfEvents = events.ToList();
            var lastEventBeforeBreak = listOfEvents[0];
            var firstEventAfterBreak = listOfEvents[1];
            var maxDuration = TimeSpan.Zero;
            for (var i = 1; i < listOfEvents.Count - 1; i++)
            {
                var duration = listOfEvents[i].HappensDate - listOfEvents[i + 1].HappensDate;
                if (duration > maxDuration)
                {
                    maxDuration = duration;
                    lastEventBeforeBreak = listOfEvents[i];
                    firstEventAfterBreak = listOfEvents[i + 1];
                }
            }

            return (lastEventBeforeBreak, firstEventAfterBreak);
        }

        private static double GetAverageDurationBetweenEvents(IReadOnlyCollection<Event> events)
        {
            var numberOfBreaks = events.Count - 1;
            return (double) (events.First().HappensDate - events.Last().HappensDate).Days / numberOfBreaks;
        }
    }
}