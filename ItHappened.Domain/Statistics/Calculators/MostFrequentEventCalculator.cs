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
            if (!CanCalculate(eventTrackers))
                return Option<IStatisticsFact>.None;

            var trackingNameWithEventsPeriod = eventTrackers
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

            var (trackingName, eventsPeriod) = trackingNameWithEventsPeriod
                .OrderBy(e => e.eventsPeriod)
                .FirstOrDefault();

            return Option<IStatisticsFact>
                .Some(new MostFrequentEventFact(trackingName, eventsPeriod, trackingNameWithEventsPeriod));
        }

        private bool CanCalculate(IEnumerable<EventTracker> eventTrackers)
        {
            return eventTrackers.Count() > 1 && eventTrackers.Count(et => et.Events.Count > 3) > 1;
        }
    }
}