using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class MultipleTrackersEventsCountCalculator : IMultipleTrackersStatisticsCalculator
    {
        private const int EventsThreshold = 5;
        private const int TrackersThreshold = 0;

        public Option<IMultipleTrackersFact> Calculate(
            IReadOnlyCollection<TrackerWithItsEvents> trackerWithItsEvents, DateTimeOffset now)
        {
            var allEvents = trackerWithItsEvents.SelectMany(info => info.Events).ToList();
            if (!CanCalculate(trackerWithItsEvents, allEvents))
                return Option<IMultipleTrackersFact>.None;

            const string factName = "Зафиксировано уже N событий";
            var eventsCount = allEvents.Count;
            var description = $"У вас произошло уже {eventsCount} событий!";
            var priority = Math.Log(eventsCount);

            return Option<IMultipleTrackersFact>.Some(
                new EventsCountTrackersFact(
                    factName,
                    description,
                    priority,
                    eventsCount));
        }

        private static bool CanCalculate(IReadOnlyCollection<TrackerWithItsEvents> trackerWithItsEvents,
            ICollection allEvents)
        {
            return trackerWithItsEvents.Count > TrackersThreshold &&
                   allEvents.Count > EventsThreshold;
        }
    }
}