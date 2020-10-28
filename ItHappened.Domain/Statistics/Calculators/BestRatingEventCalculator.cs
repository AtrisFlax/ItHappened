using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class BestRatingEventCalculator : ISingleTrackerStatisticsCalculator
    {
        private const int MinDaysThreshold = 7;
        private const int MaxMonthThreshold = 3;
        private const int ThresholdEventsWithRating = 10;
        private BestRatingEventInfo _bestRatingEventInfo;

        public Option<ISingleTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker,
            DateTimeOffset now)
        {
            if (!CanCalculate(events, now))
            {
                return Option<ISingleTrackerFact>.None;
            }

            var bestRatingEvent = _bestRatingEventInfo.Event;
            var comment = bestRatingEvent.CustomizationsParameters.Comment;
            var bestRating = _bestRatingEventInfo.Rating;
            var commentInfo = comment.IfNone(new Comment(string.Empty));
            var textComment = $" с комментарием {commentInfo.Text}";
            const string factName = "Лучшее событие";
            var description = $"Событие {tracker.Name} с самым высоким рейтингом {bestRating} " +
                              $"произошло {bestRatingEvent.HappensDate:d}{textComment}";
            var priority = bestRating;
            var bestEventDate = _bestRatingEventInfo.Event.HappensDate;

            return Option<ISingleTrackerFact>.Some(new BestRatingEventFact(
                factName,
                description,
                priority,
                bestRating,
                bestEventDate,
                comment));
        }

        private bool CanCalculate(IReadOnlyCollection<Event> events, DateTimeOffset now)
        {
            var eventEnough = CountEventWithRating(events);
            if (eventEnough <= ThresholdEventsWithRating)
            {
                return false;
            }

            var earliestEventDate = EarliestEventDate(events);
            if (now.AddMonths(-MaxMonthThreshold) < earliestEventDate)
            {
                return false;
            }

            _bestRatingEventInfo
                = events
                    .Where(@event => @event.CustomizationsParameters.Rating.IsSome)
                    .Select(x => new BestRatingEventInfo
                    {
                        Event = x,
                        Rating = x.CustomizationsParameters.Rating.ValueUnsafe()
                    })
                    .OrderByDescending(x => x.Rating).First();

            if (_bestRatingEventInfo.Event.HappensDate > now.AddDays(-MinDaysThreshold))
            {
                return false;
            }

            return true;
        }

        private static DateTimeOffset EarliestEventDate(IReadOnlyCollection<Event> events)
        {
            return events
                .OrderBy(eventItem => eventItem.HappensDate)
                .First()
                .HappensDate;
        }

        private static int CountEventWithRating(IReadOnlyCollection<Event> events)
        {
            var eventEnough = events
                .Select(@event => @event.CustomizationsParameters.Rating)
                .Somes()
                .Count();
            return eventEnough;
        }

        private class BestRatingEventInfo
        {
            public Event Event { get; set; }
            public double Rating { get; set; }
        }
    }
}