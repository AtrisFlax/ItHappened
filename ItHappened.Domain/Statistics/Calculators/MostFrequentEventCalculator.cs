using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class MostFrequentEventCalculator : IGeneralCalculator
    {
        private readonly IEventRepository _eventRepository;
        private const int PriorityCoefficient = 10;

        public MostFrequentEventCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Option<IGeneralFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            var trackers = eventTrackers.ToList();
            if (!CanCalculate(trackers))
            {
                return Option<IGeneralFact>.None;
            }

            var trackingNameWithEventsPeriod = trackers
                .Select(eventTracker => (trackingName: eventTracker.Name, eventsPeriod: 1.0 * (DateTime.Now -
                        _eventRepository.LoadAllTrackerEvents(eventTracker.Id)
                            .OrderBy(e => e.HappensDate)
                            .First()
                            .HappensDate)
                    .TotalDays / _eventRepository.LoadAllTrackerEvents(eventTracker.Id).Count)
                );

            var eventTrackersWithPeriods = trackingNameWithEventsPeriod.ToList();
            var (trackingName, eventsPeriod) = eventTrackersWithPeriods
                .OrderBy(e => e.eventsPeriod)
                .FirstOrDefault();
            return Option<IGeneralFact>.Some(new MostFrequentEventFact(
                "Самое частое событие",
                $"Чаще всего у вас происходит событие {trackingName} - раз в {eventsPeriod:0.#} дней",
                PriorityCoefficient / eventsPeriod,
                trackingName,
                eventsPeriod
            ));
        }

        private bool CanCalculate(IReadOnlyCollection<EventTracker> eventTrackers)
        {
            return eventTrackers.Count > 1 &&
                   eventTrackers.All(tracker => _eventRepository.LoadAllTrackerEvents(tracker.Id).Count > 3);
        }
    }
}