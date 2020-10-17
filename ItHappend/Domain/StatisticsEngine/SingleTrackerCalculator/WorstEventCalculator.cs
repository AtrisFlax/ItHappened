using System;
using System.Linq;
using ItHappend.Domain.EventCustomization;
using ItHappend.Domain.Statistics.StatisticsFacts;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappend.Domain.Statistics.SingleTrackerCalculator
{
    public class WorstEventCalculator
    {
        public Option<WorstEvent> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<WorstEvent>.None;

            var worstEvent = eventTracker.Events.OrderBy(eventItem => eventItem.Rating).First();
            var priority = 10 - worstEvent.Rating.Value();
            var description =
                $"Событие в отслеживании {eventTracker.Name} с самым низким рейтингом {worstEvent.Rating} " +
                $"произошло {worstEvent.HappensDate} с комментарием {worstEvent.Comment}";
            var comment = worstEvent.Comment.IsSome
                ? new Comment(worstEvent.Comment.ValueUnsafe().ToString())
                : null;
            return Option<WorstEvent>.Some(new WorstEvent(description,
                priority,
                worstEvent.HappensDate,
                comment,
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