using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class WorstEventCalculator : ISingleTrackerStatisticsCalculator
    {
        private const int MinNumberOfEventsWithRating = 10;
        private const int MinNumberOfDaysFromOldestEvent = 90;
        private const int MinNumberOfDaysFromOccurenceOfEventWithLowestRating = 7;
        
        public Option<ISingleTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            if (!CanCalculate(events)) return Option<ISingleTrackerFact>.None;
            const string factName = "Худшее событие";
            var worstEvent = events
                .OrderBy(eventItem => eventItem.CustomizationsParameters.Rating)
                .First();
            var rating = worstEvent.CustomizationsParameters.Rating.Value();
            var priority = 10 - rating;
            var worstEventComment = worstEvent.CustomizationsParameters.Comment.Match(
                comment => comment.Text,
                () => string.Empty);
            var description = $"Событие в отслеживании {tracker.Name} с самым низким рейтингом " +
                              $"{rating} произошло {worstEvent.HappensDate} " +
                              $"с комментарием {worstEventComment}";

            return Option<ISingleTrackerFact>.Some(new WorstEventTrackerFact(
                factName,
                description,
                priority,
                worstEvent.CustomizationsParameters.Rating.Value(),
                worstEvent.HappensDate,
                new Comment(worstEventComment),
                worstEvent));
        }

        private bool CanCalculate(IReadOnlyCollection<Event> events)
        {
            var isEventsNumberWithRatingMoreOrEqualToTen = events
                .Count(eventItem => eventItem.CustomizationsParameters.Rating.IsSome) >= MinNumberOfEventsWithRating;
            var isOldestEventHappenedMoreThanThreeMonthsAgo = events
                .OrderBy(eventItem => eventItem.HappensDate)
                .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(MinNumberOfDaysFromOldestEvent);
            var isEventWithLowestRatingHappenedMoreThanWeekAgo = events
                .OrderBy(eventItem => eventItem.CustomizationsParameters.Rating)
                .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(MinNumberOfDaysFromOccurenceOfEventWithLowestRating);
            return isEventsNumberWithRatingMoreOrEqualToTen &&
                   isOldestEventHappenedMoreThanThreeMonthsAgo &&
                   isEventWithLowestRatingHappenedMoreThanWeekAgo;
        }
    }
}