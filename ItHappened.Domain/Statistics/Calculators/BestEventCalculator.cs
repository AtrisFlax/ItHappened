using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class BestEventCalculator : ISingleTrackerStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;
        public BestEventCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public Option<IStatisticsFact> Calculate(EventTracker eventTracker)
        {
            var trackerEvents=_eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            if (!CanCalculate(eventTracker, trackerEvents)) return Option<IStatisticsFact>.None;
            const string factName = "Лучшее событие";
            var bestEvent = trackerEvents
                .OrderBy(eventItem => eventItem.Rating).Last();
            var priority = bestEvent.Rating.Value();
            var bestEventComment = bestEvent.Comment.Match(
                comment => comment.Text,
                () => string.Empty);
            var description = $"Событие {eventTracker.Name} с самым высоким рейтингом {bestEvent.Rating} " +
                              $"произошло {bestEvent.HappensDate} с комментарием {bestEventComment}";

            return Option<IStatisticsFact>.Some(new BestEventFact(
                factName,
                description,
                priority,
                bestEvent.Rating.Value(),
                bestEvent.HappensDate,
                new Comment(bestEventComment),
                bestEvent));
        }

        private bool CanCalculate(EventTracker eventTracker, IReadOnlyList<Event> trackerEvents)
        {
            var isEventsNumberWithRatingMoreOrEqualToTen = trackerEvents
                .Count(eventItem => eventItem.Rating.IsSome) >= 10;
            var isOldestEventHappenedMoreThanThreeMonthsAgo = trackerEvents
                .OrderBy(eventItem => eventItem.HappensDate)
                .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(90);
            var isEventWithLowestRatingHappenedMoreThanWeekAgo = trackerEvents
                .OrderBy(eventItem => eventItem.Rating)
                .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(7);
            return isEventsNumberWithRatingMoreOrEqualToTen &&
                   isOldestEventHappenedMoreThanThreeMonthsAgo &&
                   isEventWithLowestRatingHappenedMoreThanWeekAgo;
        }
    }
}