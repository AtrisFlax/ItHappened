using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class BestRatingCalculator : ISingleTrackerStatisticsCalculator
    {
        private const int MinDaysThreshold = 7;
        private const int MaxMonthThreshold = 3;
        private const int ThresholdEventsWithRating = 10;

        public Option<ISingleTrackerTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            var now = DateTimeOffset.Now;
            if (!CanCalculate(events, now))
            {
                return Option<ISingleTrackerTrackerFact>.None;
            }

            var bestRatingEventInfo
                = events
                    .Where(@event => @event.CustomizationsParameters.Rating.IsSome)
                    .Select(x => new
                    {
                        Event = x,
                        Rating = x.CustomizationsParameters.Rating.ValueUnsafe()
                    })
                    .OrderBy(x => x.Rating).Last();

            if (bestRatingEventInfo.Event.HappensDate < now.AddDays(-MinDaysThreshold))
            {
                return Option<ISingleTrackerTrackerFact>.None;
            }

            var bestRatingEvent = bestRatingEventInfo.Event;
            var comment = bestRatingEvent.CustomizationsParameters.Comment;
            var bestRating = bestRatingEventInfo.Rating;
            var commentInfo = comment.Match(
                comm => $" с комментарием {comm}",
                () => string.Empty);
            const string factName = "Лучшее событие";
            var description = $"Событие {tracker.Name} с самым высоким рейтингом {bestRating} " +
                              $"произошло {bestRatingEvent.HappensDate}{commentInfo}";
            var priority = bestRating;
            var bestEventDate = bestRatingEventInfo.Event.HappensDate;

            return Option<ISingleTrackerTrackerFact>.Some(new BestEventTrackerFact(
                factName,
                description,
                priority,
                bestRating,
                bestEventDate,
                comment));
        }

        private static bool CanCalculate(IReadOnlyCollection<Event> events, DateTimeOffset now)
        {
            var eventEnough = CountEventWithRating(events);
            if (eventEnough <= ThresholdEventsWithRating)
            {
                return false;
            }

            var isRatherEventHappenedMoreThanThreeMonthsAgo = EarliestEventDate(events);
            if (now.AddMonths(-MaxMonthThreshold) >= isRatherEventHappenedMoreThanThreeMonthsAgo)
            {
                return false;
            }

            return true;
        }

        private static DateTimeOffset EarliestEventDate(IReadOnlyCollection<Event> events)
        {
            var isRatherEventHappenedMoreThanThreeMonthsAgo = events
                .OrderBy(eventItem => eventItem.HappensDate)
                .First()
                .HappensDate;
            return isRatherEventHappenedMoreThanThreeMonthsAgo;
        }

        private static int CountEventWithRating(IReadOnlyCollection<Event> events)
        {
            var eventEnough = events
                .Select(@event => @event.CustomizationsParameters.Rating)
                .Somes()
                .Count();
            return eventEnough;
        }
    }
}