using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class BestRatingEventCalculator : ISingleTrackerStatisticsCalculator
    {
        private static int MaxDateThreshold;
        private const int TresholdEventsWithRating = 10;

        public Option<ISingleTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            if (!CanCalculate(events))
            {
                return Option<ISingleTrackerFact>.None;
            }

            var bestRatingEventInfo
                = events
                .Where(@event => @event.CustomizationsParameters.Rating.IsSome)
                .Select(x => new
                {
                    Event = x,
                    Rating = x.CustomizationsParameters.Rating.ValueUnsafe()
                })
                .OrderBy(x=>x.Rating).Last();
            
            var bestRatingEvent = bestRatingEventInfo.Event;
            var comment = bestRatingEvent.CustomizationsParameters.Comment;
            var bestRating = bestRatingEventInfo.Rating;
            var bestEventComment = comment.Match(
                comment => comment.Text,
                () => string.Empty);
            var commentInfo = bestEventComment == string.Empty ? " с комментарием {bestEventComment}}" : "";
            const string factName = "Лучшее событие";
            var description = $"Событие {tracker.Name} с самым высоким рейтингом {bestRating} " +
                                    $"произошло {bestRatingEvent.HappensDate} {commentInfo}";
            var priority = bestRating;
            var bestEventDate = bestRatingEventInfo.Event.HappensDate;
            return Option<ISingleTrackerFact>.Some(new BestEventTrackerFact(
                factName,
                description,
                priority,
                bestRating,
                bestEventDate,
                comment));
        }
        
        private static bool CanCalculate(IReadOnlyCollection<Event> events)
        {
            var isEventsNumberWithRatingMoreOrEqualToTen = events
                .Count(@event => @event.CustomizationsParameters.Rating.IsSome) >= TresholdEventsWithRating;
            MaxDateThreshold = 90;
            var isOldestEventHappenedMoreThanThreeMonthsAgo = events
                .OrderBy(eventItem => eventItem.HappensDate)
                .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(MaxDateThreshold);
            var isEventWithLowestRatingHappenedMoreThanWeekAgo = events
                .OrderBy(eventItem => eventItem.CustomizationsParameters.Rating)
                .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(7);
            return isEventsNumberWithRatingMoreOrEqualToTen &&
                   isOldestEventHappenedMoreThanThreeMonthsAgo &&
                   isEventWithLowestRatingHappenedMoreThanWeekAgo;
        }
    }
}