using System.Collections.Generic;

namespace ItHappened.Domain.Statistics.Facts.ForMultipleTrackers
{
    public class MostFrequentEvent : IMultipleTrackersStatisticsFact
    {
        public MostFrequentEvent(IReadOnlyCollection<(EventTracker, double)> eventTrackersWithPeriods,
            EventTracker eventTrackerWithSmallestEventPeriod,
            double smallestEventsPeriod)
        {
            EventTrackerWithSmallestEventPeriod = eventTrackerWithSmallestEventPeriod;
            EventTrackersWithPeriods = eventTrackersWithPeriods;
            SmallestEventsPeriod = smallestEventsPeriod;
        }

        public string FactName { get; }
        public string Description => $"Чаще всего у вас происходит событие " +
                                     $"{EventTrackerWithSmallestEventPeriod.Name}" +
                                     $" - раз в {SmallestEventsPeriod} дней";

        public double Priority => 10 / SmallestEventsPeriod;
        public EventTracker EventTrackerWithSmallestEventPeriod  { get; }
        public IReadOnlyCollection<(EventTracker, double)> EventTrackersWithPeriods { get; }
        public double SmallestEventsPeriod { get; }
    }
}