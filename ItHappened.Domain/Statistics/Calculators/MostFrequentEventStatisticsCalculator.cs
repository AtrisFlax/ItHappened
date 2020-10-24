using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class MostFrequentEventStatisticsCalculator : IMultipleTrackersStatisticsCalculator
    {
        private const int ThresholdEvents = 3;
        private const int ThresholdTrackers = 2;

        public Option<IMultipleTrackersFact> Calculate(
            IReadOnlyCollection<TrackerWithItsEvents> trackerWithItsEvents)
        {
            if (!CanCalculate(trackerWithItsEvents))
            {
                return Option<IMultipleTrackersFact>.None;
            }

            var nowTime = DateTimeOffset.Now;
            var trackingNameWithEventsPeriod = trackerWithItsEvents
                .Select(x => (
                        trackingName: x.Tracker.Name,
                        eventsPeriod: 1.0 *
                        (nowTime - x.Events.OrderBy(e => e.HappensDate).First().HappensDate).TotalDays / x.Events.Count
                    )
                );
            var (trackingName, eventsPeriod) = trackingNameWithEventsPeriod
                .OrderBy(e => e.eventsPeriod)
                .FirstOrDefault();
            return Option<IMultipleTrackersFact>.Some(new MostFrequentEventTrackersFact(
                "Самое частое событие",
                $"Чаще всего у вас происходит событие {trackingName} - раз в {eventsPeriod:0.#} дней",
                10 / eventsPeriod,
                trackingName,
                eventsPeriod
            ));
        }

        private static bool CanCalculate(IReadOnlyCollection<TrackerWithItsEvents> trackerWithItsEvents)
        {
            return trackerWithItsEvents
                .Select(x => x.Events.Count > ThresholdEvents)
                .Count(x => x.Equals(true)) >= ThresholdTrackers;
        }
    }
}