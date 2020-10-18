using System;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappend.Domain.Statistics
{
    public class WorstEventCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerStatisticsFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<ISingleTrackerStatisticsFact>.None;
            const string factName = "Худшее событие";
            var worstEvent = eventTracker.Events.OrderBy(eventItem => eventItem.Rating).First();
            var priority = 10 - worstEvent.Rating.Value();
            var worstEventComment = worstEvent.Comment.Match(
                Some: comment => comment,
                None: () => Option<Comment>.None);
            var description = $"Событие в отслеживании {eventTracker.Name} с самым низким " +
                                 $"рейтингом {worstEvent.Rating} произошло {worstEvent.HappensDate}";
            description += worstEventComment.IsSome ?
                $" с комментарием {worstEventComment.ValueUnsafe().Text}" :
                " (комментарий отсутсвует)";

            return Option<ISingleTrackerStatisticsFact>.Some(new WorstEventFact(
                factName,
                description,
                priority,
                worstEvent.Rating.Value(),
                worstEvent.HappensDate,
                worstEventComment,
                worstEvent));
        }

        private bool CanCalculate(EventTracker eventTracker)
        {
            var isEventsNumberWithRatingMoreOrEqualToTen = eventTracker.Events
                .Count(eventItem => eventItem.Rating.IsSome) >= 10;
            var isOldestEventHappenedMoreThanThreeMonthsAgo = eventTracker.Events
                                                                  .OrderBy(eventItem => eventItem.HappensDate)
                                                                  .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(90);
            var isEventWithLowestRatingHappenedMoreThanWeekAgo = eventTracker.Events
                                                                     .OrderBy(eventItem => eventItem.Rating)
                                                                     .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(7);
            return isEventsNumberWithRatingMoreOrEqualToTen && 
                   isOldestEventHappenedMoreThanThreeMonthsAgo &&
                   isEventWithLowestRatingHappenedMoreThanWeekAgo;
        }
    }
}