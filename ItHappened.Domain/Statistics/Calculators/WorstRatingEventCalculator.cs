﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class WorstRatingEventCalculator : ISingleTrackerStatisticsCalculator
    {
        private const int MinDaysThreshold = 7;
        private const int MaxMonthThreshold = 3;
        private const int ThresholdEventsWithRating = 10;
        private const double MaxRating = 10.0;

        public Option<ISingleTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker, DateTimeOffset now)
        {
            if (!CanCalculate(events, now))
            {
                return Option<ISingleTrackerFact>.None;
            }

            var worstRatingEventInfo
                = events
                    .Where(@event => @event.CustomizationsParameters.Rating.IsSome)
                    .Select(x => new
                    {
                        Event = x,
                        Rating = x.CustomizationsParameters.Rating.ValueUnsafe()
                    })
                    .OrderBy(x => x.Rating).First();

            if (worstRatingEventInfo.Event.HappensDate > now.AddDays(-MinDaysThreshold))
            {
                 return Option<ISingleTrackerFact>.None;
            }

            var worstRatingEvent = worstRatingEventInfo.Event;    
            var comment = worstRatingEvent.CustomizationsParameters.Comment;
            var worstRating = worstRatingEventInfo.Rating;
            var commentInfo = comment.IfNone(new Comment(string.Empty));
            var textComment = $" с комментарием {commentInfo.Text}";
            const string factName = "Худшее событие";
            var description = $"Событие {tracker.Name} с самым низким рейтингом {worstRating} " +
                              $"произошло {worstRatingEvent.HappensDate:d}{textComment}";
            var priority = MaxRating -  worstRating;
            var worstEventDate = worstRatingEventInfo.Event.HappensDate;

            return Option<ISingleTrackerFact>.Some(new WorstRatingEventFact(
                factName,
                description,
                priority,
                worstRating,
                worstEventDate,
                commentInfo,
                worstRatingEvent.Id));
        }

        private static bool CanCalculate(IReadOnlyCollection<Event> events, DateTimeOffset now)
        {
            var eventEnough = CountEventWithRating(events);
            if (eventEnough <= ThresholdEventsWithRating)
            {
                return false;
            }

            var isRatherEventHappenedMoreThanThreeMonthsAgo = EarliestEventDate(events);
            if (now.AddMonths(-MaxMonthThreshold) > isRatherEventHappenedMoreThanThreeMonthsAgo)
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