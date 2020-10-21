using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class MultipleTrackersEventsCountCalculator : IGeneralCalculator
    {
        private const int EventsThreshold = 5;
        private readonly IEventRepository _eventRepository;

        public MultipleTrackersEventsCountCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Option<IGeneralFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            var enumerable = eventTrackers.ToList();
            if (!CanCalculate(enumerable))
                return Option<IGeneralFact>.None;

            var factName = "Зафиксировано уже N событий";
            var eventsCount = enumerable.Sum(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id).Count);
            var description = $"У вас произошло уже {eventsCount} событий!";
            var priority = Math.Log(eventsCount);

            return Option<IGeneralFact>.Some(new EventsCountFact(factName, description, priority, eventsCount));
        }

        private bool CanCalculate(IEnumerable<EventTracker> eventTrackers)
        {
            var enumerable = eventTrackers.ToList();
            var eventsCount = enumerable.Sum(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id).Count);
            return enumerable.Any() &&
                   eventsCount > EventsThreshold;
        }
    }
}