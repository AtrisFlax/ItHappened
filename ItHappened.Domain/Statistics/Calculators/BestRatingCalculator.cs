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

        public Option<ISingleTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            var now = DateTimeOffset.Now;
            if (!CanCalculate(events, now))
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
                    .OrderByDescending(x => x.Rating).First();

            if (bestRatingEventInfo.Event.HappensDate < now.AddDays(-MinDaysThreshold))
            {
                 return Option<ISingleTrackerFact>.None;
            }

            var bestRatingEvent = bestRatingEventInfo.Event;
            var comment = bestRatingEvent.CustomizationsParameters.Comment;
            var bestRating = bestRatingEventInfo.Rating;
            var commentInfo = comment.IfNone(new Comment(string.Empty));
            var textComment = $" с комментарием {commentInfo.Text}";
            const string factName = "Лучшее событие";
            var description = $"Событие {tracker.Name} с самым высоким рейтингом {bestRating} " +
                              $"произошло {bestRatingEvent.HappensDate:d}{textComment}";
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