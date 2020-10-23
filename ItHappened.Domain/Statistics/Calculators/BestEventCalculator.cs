using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class BestEventCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            if (!CanCalculate(events)) return Option<ISingleTrackerTrackerFact>.None;
            const string factName = "Лучшее событие";
            var bestEvent = events
                .OrderBy(eventItem => eventItem.CustomizationsParameters.Rating).Last();
            var priority = bestEvent.CustomizationsParameters.Rating.Value();
            var bestEventComment = bestEvent.CustomizationsParameters.Comment.Match(
                comment => comment.Text,
                () => string.Empty);
            var description =
                $"Событие {tracker.Name} с самым высоким рейтингом {bestEvent.CustomizationsParameters.Rating} " +
                $"произошло {bestEvent.HappensDate} с комментарием {bestEventComment}";
            return Option<ISingleTrackerTrackerFact>.Some(new BestEventTrackerFact(
                factName,
                description,
                priority,
                bestEvent.CustomizationsParameters.Rating.Value(),
                bestEvent.HappensDate,
                new Comment(bestEventComment),
                bestEvent));
        }

        private static bool CanCalculate(IReadOnlyCollection<Event> events)
        {
            var isEventsNumberWithRatingMoreOrEqualToTen = events
                .Count(eventItem => eventItem.CustomizationsParameters.Rating.IsSome) >= 10;
            var isOldestEventHappenedMoreThanThreeMonthsAgo = events
                .OrderBy(eventItem => eventItem.HappensDate)
                .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(90);
            var isEventWithLowestRatingHappenedMoreThanWeekAgo = events
                .OrderBy(eventItem => eventItem.CustomizationsParameters.Rating)
                .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(7);
            return isEventsNumberWithRatingMoreOrEqualToTen &&
                   isOldestEventHappenedMoreThanThreeMonthsAgo &&
                   isEventWithLowestRatingHappenedMoreThanWeekAgo;
        }
    }
}