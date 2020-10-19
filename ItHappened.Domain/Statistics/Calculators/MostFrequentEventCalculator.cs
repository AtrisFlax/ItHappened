using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class MostFrequentEventCalculator : IMultipleTrackersStatisticsCalculator
    {
        public Option<IStatisticsFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            var eventsTracks = eventTrackers.ToList();
            if (!CanCalculate(eventsTracks.ToList()))
                return Option<IStatisticsFact>.None;

            var trackingNameWithEventsPeriod = eventsTracks
                .Select(eventTracker =>
                    (trackingName: eventTracker.Name,
                        eventsPeriod: 1.0 *
                        (DateTime.Now - eventTracker
                            .Events
                            .OrderBy(e => e.HappensDate)
                            .First()
                            .HappensDate)
                        .TotalDays / eventTracker.Events.Count)
                );

            var eventTrackersWithPeriods = trackingNameWithEventsPeriod.ToList();
            var (trackingName, eventsPeriod) = eventTrackersWithPeriods
                .OrderBy(e => e.eventsPeriod)
                .FirstOrDefault();

            return Option<IStatisticsFact>
                .Some(new MostFrequentEventFact(trackingName, eventsPeriod, eventTrackersWithPeriods));
        }

        private static bool CanCalculate(IList<EventTracker> eventTrackers)
        {
            return eventTrackers.Count() > 1 && eventTrackers.Count(et => et.Events.Count > 3) > 1;
        }
    }
}