using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class MostFrequentEventCalculator : IMultipleTrackersStatisticsCalculator
    {
        private readonly IEventRepository _eventRepository;
        public MostFrequentEventCalculator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public Option<IStatisticsFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            var eventsTracks = eventTrackers.ToList();
            if (!CanCalculate(eventsTracks.ToList()))
                return Option<IStatisticsFact>.None;

            var trackingNameWithEventsPeriod = eventsTracks
                .Select(eventTracker =>
                    (trackingName: eventTracker.Name,
                        eventsPeriod: 1.0 *
                        (DateTime.Now - _eventRepository.LoadAllTrackerEvents(eventTracker.Id)
                            .OrderBy(e => e.HappensDate)
                            .First()
                            .HappensDate)
                        .TotalDays / _eventRepository.LoadAllTrackerEvents(eventTracker.Id).Count)
                );

            var eventTrackersWithPeriods = trackingNameWithEventsPeriod.ToList();
            var (trackingName, eventsPeriod) = eventTrackersWithPeriods
                .OrderBy(e => e.eventsPeriod)
                .FirstOrDefault();

            return Option<IStatisticsFact>
                .Some(new MostFrequentEventFact(trackingName, eventsPeriod, eventTrackersWithPeriods));
        }

        private bool CanCalculate(IList<EventTracker> eventTrackers)
        {
            return eventTrackers.Count() > 1 && eventTrackers
                .Count(et => _eventRepository.LoadAllTrackerEvents(et.Id).Count > 3) > 1;
        }
    }
}