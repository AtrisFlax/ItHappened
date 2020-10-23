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
        public Option<ISingleTrackerFact> Calculate(EventTracker eventTracker)
        {
            var trackerEvents=_eventRepository.LoadAllTrackerEvents(eventTracker.Id);
            if (!CanCalculate(eventTracker, trackerEvents)) return Option<ISingleTrackerFact>.None;
            const string factName = "Лучшее событие";
            var bestEvent = trackerEvents
                .OrderBy(eventItem => eventItem.CustomParameters.Rating).Last();
            var priority = bestEvent.CustomParameters.Rating.Value();
            var bestEventComment = bestEvent.CustomParameters.Comment.Match(
                comment => comment.Text,
                () => string.Empty);
            var description = $"Событие {eventTracker.Name} с самым высоким рейтингом {bestEvent.CustomParameters.Rating} " +
                              $"произошло {bestEvent.HappensDate} с комментарием {bestEventComment}";

            return Option<ISingleTrackerFact>.Some(new BestEventFact(
                factName,
                description,
                priority,
                bestEvent.CustomParameters.Rating.Value(),
                bestEvent.HappensDate,
                new Comment(bestEventComment),
                bestEvent));
        }

        private bool CanCalculate(EventTracker eventTracker, IReadOnlyList<Event> trackerEvents)
        {
            var isEventsNumberWithRatingMoreOrEqualToTen = trackerEvents
                .Count(eventItem => eventItem.CustomParameters.Rating.IsSome) >= 10;
            var isOldestEventHappenedMoreThanThreeMonthsAgo = trackerEvents
                .OrderBy(eventItem => eventItem.HappensDate)
                .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(90);
            var isEventWithLowestRatingHappenedMoreThanWeekAgo = trackerEvents
                .OrderBy(eventItem => eventItem.CustomParameters.Rating)
                .First().HappensDate <= DateTimeOffset.Now - TimeSpan.FromDays(7);
            return isEventsNumberWithRatingMoreOrEqualToTen &&
                   isOldestEventHappenedMoreThanThreeMonthsAgo &&
                   isEventWithLowestRatingHappenedMoreThanWeekAgo;
        }
    }
}