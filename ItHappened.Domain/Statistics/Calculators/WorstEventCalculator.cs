using System;
using System.Linq;
using ItHappend.Domain.Statistics;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics
{
    public class WorstEventCalculator : ISingleTrackerStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;
        
        public WorstEventCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        
        public Option<ISingleTrackerFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<ISingleTrackerFact>.None;
            const string factName = "Худшее событие";
            var worstEvent = _eventRepository.LoadAllTrackerEvents(eventTracker.Id)
                .OrderBy(eventItem => eventItem.CustomParameters.Rating)
                .First();
            var priority = 10 - worstEvent.CustomParameters.Rating.Value();
            var worstEventComment = worstEvent.CustomParameters.Comment.Match(
                comment => comment.Text,
                () => string.Empty);
            var description = $"Событие в отслеживании {eventTracker.Name} с самым низким рейтингом " +
                              $"{worstEvent.CustomParameters.Rating} произошло {worstEvent.HappensDate} " +
                              $"с комментарием {worstEventComment}";

            return Option<ISingleTrackerFact>.Some(new WorstEventFact(
                factName,
                description,
                priority,
                worstEvent.CustomParameters.Rating.Value(),
                worstEvent.HappensDate,
                new Comment(worstEventComment),
                worstEvent));
        }

        private bool CanCalculate(EventTracker eventTracker)
        {
            var trackerEvents=_eventRepository.LoadAllTrackerEvents(eventTracker.Id);
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