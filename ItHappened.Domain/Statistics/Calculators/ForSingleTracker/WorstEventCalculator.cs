using System;
using System.Linq;
using ItHappend.Domain.Statistics.StatisticsFacts;
using ItHappened.Domain;
using ItHappened.Domain.EventCustomization;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappend.Domain.Statistics.SingleTrackerCalculator
{
    public class WorstEventCalculator
    {
        public Option<WorstEventFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<WorstEventFact>.None;
            const string factName = "Худшее событие";
            var worstEvent = eventTracker.Events.OrderBy(eventItem => eventItem.Rating).First();
            var priority = 10 - worstEvent.Rating.Value();
            var worstEventComment = worstEvent.Comment.Match(
                Some: comment => comment,
                None: () => Option<Comment>.None);
            string description = $"Событие в отслеживании {eventTracker.Name} с самым низким " +
                                 $"рейтингом {worstEvent.Rating} произошло {worstEvent.HappensDate}";
            description += worstEventComment.IsSome ?
                $" с комментарием {worstEventComment.ValueUnsafe().Text}" :
                " (комментарий отсутсвует)";

            return Option<WorstEventFact>.Some(new WorstEventFact(
                factName,
                description,
                priority,
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