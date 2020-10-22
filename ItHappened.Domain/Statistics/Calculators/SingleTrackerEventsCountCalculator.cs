using System;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class SingleTrackerEventsCountCalculator : ISingleTrackerStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;
        
        public SingleTrackerEventsCountCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        
        public Option<ISingleTrackerFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker))
                return Option<ISingleTrackerFact>.None;

            const string factName = "Количество событий";
            var eventsCount = _eventRepository.LoadAllTrackerEvents(eventTracker.Id).Count;
            var description = $"Событие {eventTracker.Name} произошло {eventsCount} раз";
            var priority = Math.Log(eventsCount);

            return Option<ISingleTrackerFact>
                .Some(new SingleTrackerEventsCountFact(factName, description, priority, eventsCount));
        }

        private bool CanCalculate(EventTracker eventTracker)
        {
            return _eventRepository.LoadAllTrackerEvents(eventTracker.Id).Count > 0;
        }
    }
}